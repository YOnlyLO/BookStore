import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { getErrorMessage, getResourceError } from '../../../core/http/api-error';
import { Order } from '../../../core/models/order.model';
import { BookService } from '../../../core/services/book.service';
import { OrderService } from '../../../core/services/order.service';
import { UserService } from '../../../core/services/user.service';
import { getFieldError } from '../../../shared/forms/form-validation';
import { ConfirmDialogComponent } from '../../../shared/ui/confirm-dialog/confirm-dialog.component';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ModalComponent } from '../../../shared/ui/modal/modal.component';
import { OrderEditorComponent } from './order-editor/order-editor.component';
import { OrderStatusBadgeComponent } from './order-status-badge.component';

@Component({
  selector: 'app-orders-admin',
  imports: [
    CurrencyPipe,
    DatePipe,
    ReactiveFormsModule,
    IconComponent,
    ModalComponent,
    ConfirmDialogComponent,
    OrderEditorComponent,
    OrderStatusBadgeComponent
  ],
  templateUrl: './orders-admin.component.html',
  host: { class: 'page' }
})
export class OrdersAdminComponent {
  private readonly orderService = inject(OrderService);
  private readonly userService = inject(UserService);
  private readonly bookService = inject(BookService);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly orders = rxResource({
    stream: () => this.orderService.getAll(),
    defaultValue: []
  });

  private readonly usersResource = rxResource({
    stream: () => this.userService.getAll(),
    defaultValue: []
  });

  private readonly booksResource = rxResource({
    stream: () => this.bookService.getAll(),
    defaultValue: []
  });

  protected readonly loadError = computed(() =>
    getResourceError(this.orders, this.usersResource, this.booksResource)
  );

  protected readonly rows = computed(() =>
    this.orders.hasValue()
      ? [...this.orders.value()].sort((a, b) => Date.parse(b.createdAt) - Date.parse(a.createdAt))
      : []
  );

  protected readonly users = computed(() =>
    this.usersResource.hasValue()
      ? [...this.usersResource.value()].sort((a, b) =>
          `${a.lastName} ${a.firstName}`.localeCompare(`${b.lastName} ${b.firstName}`, 'ru')
        )
      : []
  );

  protected readonly books = computed(() =>
    this.booksResource.hasValue()
      ? [...this.booksResource.value()].sort((a, b) => a.title.localeCompare(b.title, 'ru'))
      : []
  );

  protected readonly userNames = computed(
    () => new Map(this.users().map((user) => [user.id, `${user.lastName} ${user.firstName}`]))
  );

  protected readonly fieldError = getFieldError;

  protected orderNumber(order: Order): string {
    return order.id.slice(0, 8).toUpperCase();
  }

  // ----- Создание -----

  protected readonly isCreateOpen = signal(false);
  protected readonly isCreating = signal(false);
  protected readonly createError = signal<string | null>(null);

  protected readonly createForm = this.formBuilder.group({
    userId: ['', [Validators.required]]
  });

  protected openCreate(): void {
    this.createForm.reset();
    this.createError.set(null);
    this.isCreateOpen.set(true);
  }

  protected closeCreate(): void {
    this.isCreateOpen.set(false);
  }

  protected create(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isCreating.set(true);
    this.createError.set(null);

    this.orderService.create(this.createForm.getRawValue()).subscribe({
      next: (order) => {
        this.isCreating.set(false);
        this.isCreateOpen.set(false);
        this.orders.reload();
        // Заказ создаётся пустым — сразу открываем его, чтобы добавить книги.
        this.editedOrderId.set(order.id);
      },
      error: (error: unknown) => {
        this.isCreating.set(false);
        this.createError.set(getErrorMessage(error));
      }
    });
  }

  // ----- Изменение: состав и статус заказа -----

  protected readonly editedOrderId = signal<string | null>(null);

  protected openEdit(order: Order): void {
    this.editedOrderId.set(order.id);
  }

  protected closeEdit(): void {
    this.editedOrderId.set(null);
    // Состав и статус могли поменяться — обновляем список целиком.
    this.orders.reload();
  }

  // ----- Удаление -----

  protected readonly orderToDelete = signal<Order | null>(null);
  protected readonly isDeleting = signal(false);
  protected readonly deleteError = signal<string | null>(null);

  protected askDelete(order: Order): void {
    this.deleteError.set(null);
    this.orderToDelete.set(order);
  }

  protected cancelDelete(): void {
    this.orderToDelete.set(null);
  }

  protected confirmDelete(): void {
    const order = this.orderToDelete();

    if (!order) {
      return;
    }

    this.isDeleting.set(true);
    this.deleteError.set(null);

    this.orderService.delete(order.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.orderToDelete.set(null);
        this.orders.reload();
      },
      error: (error: unknown) => {
        this.isDeleting.set(false);
        this.deleteError.set(getErrorMessage(error));
      }
    });
  }

  protected reload(): void {
    this.orders.reload();
    this.usersResource.reload();
    this.booksResource.reload();
  }
}
