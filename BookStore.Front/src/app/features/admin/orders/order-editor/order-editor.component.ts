import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, input, output, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { getErrorMessage, getResourceError } from '../../../../core/http/api-error';
import { Book } from '../../../../core/models/book.model';
import { OrderItem, OrderStatus } from '../../../../core/models/order.model';
import { OrderService } from '../../../../core/services/order.service';
import { getFieldError, integer } from '../../../../shared/forms/form-validation';
import { ConfirmDialogComponent } from '../../../../shared/ui/confirm-dialog/confirm-dialog.component';
import { IconComponent } from '../../../../shared/ui/icon/icon.component';
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import { OrderStatusBadgeComponent } from '../order-status-badge.component';

/**
 * Изменение заказа. Отдельного PUT для заказа в API нет:
 * меняются состав (только в статусе «Новый») и статус — через переходы confirm/complete/cancel.
 */
@Component({
  selector: 'app-order-editor',
  imports: [
    CurrencyPipe,
    DatePipe,
    ReactiveFormsModule,
    IconComponent,
    ModalComponent,
    ConfirmDialogComponent,
    OrderStatusBadgeComponent
  ],
  templateUrl: './order-editor.component.html',
  styleUrl: './order-editor.component.css'
})
export class OrderEditorComponent {
  readonly orderId = input.required<string>();
  readonly userNames = input.required<ReadonlyMap<string, string>>();
  readonly books = input.required<Book[]>();

  readonly closed = output<void>();

  private readonly orderService = inject(OrderService);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly order = rxResource({
    params: () => this.orderId(),
    stream: ({ params: id }) => this.orderService.getById(id)
  });

  protected readonly current = computed(() =>
    this.order.hasValue() ? this.order.value() : undefined
  );

  protected readonly loadError = computed(() => getResourceError(this.order));

  protected readonly title = computed(() => `Заказ #${this.orderId().slice(0, 8).toUpperCase()}`);

  protected readonly customerName = computed(() => {
    const order = this.current();

    return order ? (this.userNames().get(order.userId) ?? 'Неизвестный пользователь') : '';
  });

  protected readonly booksById = computed(
    () => new Map(this.books().map((book) => [book.id, book]))
  );

  protected readonly isNew = computed(() => this.current()?.status === OrderStatus.New);

  protected readonly canConfirm = computed(
    () => this.isNew() && (this.current()?.items.length ?? 0) > 0
  );

  protected readonly canComplete = computed(
    () => this.current()?.status === OrderStatus.Confirmed
  );

  protected readonly canCancel = computed(() => this.isNew() || this.canComplete());

  protected readonly isBusy = signal(false);
  protected readonly actionError = signal<string | null>(null);
  protected readonly isCancelConfirmOpen = signal(false);

  protected readonly fieldError = getFieldError;

  protected readonly itemForm = this.formBuilder.group({
    bookId: ['', [Validators.required]],
    quantity: this.formBuilder.control<number | null>(1, [
      Validators.required,
      Validators.min(1),
      integer
    ]),
    unitPrice: this.formBuilder.control<number | null>(null, [
      Validators.required,
      Validators.min(0.01)
    ])
  });

  protected onBookSelected(): void {
    const book = this.booksById().get(this.itemForm.controls.bookId.value);

    if (book) {
      this.itemForm.controls.unitPrice.setValue(book.price);
    }
  }

  protected addItem(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    const value = this.itemForm.getRawValue();

    this.run(
      this.orderService.addItem(this.orderId(), {
        bookId: value.bookId,
        quantity: value.quantity ?? 1,
        unitPrice: value.unitPrice ?? 0
      }),
      () => this.itemForm.reset()
    );
  }

  protected removeItem(item: OrderItem): void {
    this.run(this.orderService.removeItem(this.orderId(), item.bookId));
  }

  protected confirmOrder(): void {
    this.run(this.orderService.confirm(this.orderId()));
  }

  protected completeOrder(): void {
    this.run(this.orderService.complete(this.orderId()));
  }

  protected askCancel(): void {
    this.actionError.set(null);
    this.isCancelConfirmOpen.set(true);
  }

  protected cancelOrder(): void {
    this.run(this.orderService.cancel(this.orderId()), () => this.isCancelConfirmOpen.set(false));
  }

  private run(request$: Observable<unknown>, onSuccess?: () => void): void {
    this.isBusy.set(true);
    this.actionError.set(null);

    request$.subscribe({
      next: () => {
        this.isBusy.set(false);
        onSuccess?.();
        this.order.reload();
      },
      error: (error: unknown) => {
        this.isBusy.set(false);
        this.actionError.set(getErrorMessage(error));
      }
    });
  }
}
