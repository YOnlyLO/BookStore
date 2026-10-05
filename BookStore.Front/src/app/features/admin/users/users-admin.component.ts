import { Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { getErrorMessage, getResourceError } from '../../../core/http/api-error';
import { User } from '../../../core/models/user.model';
import { OrderService } from '../../../core/services/order.service';
import { UserService } from '../../../core/services/user.service';
import { getFieldError, notBlank } from '../../../shared/forms/form-validation';
import { ConfirmDialogComponent } from '../../../shared/ui/confirm-dialog/confirm-dialog.component';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ModalComponent } from '../../../shared/ui/modal/modal.component';

@Component({
  selector: 'app-users-admin',
  imports: [ReactiveFormsModule, IconComponent, ModalComponent, ConfirmDialogComponent],
  templateUrl: './users-admin.component.html',
  styleUrl: './users-admin.component.css',
  host: { class: 'page' }
})
export class UsersAdminComponent {
  private readonly userService = inject(UserService);
  private readonly orderService = inject(OrderService);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly users = rxResource({
    stream: () => this.userService.getAll(),
    defaultValue: []
  });

  // Заказы нужны, чтобы показать их количество и не дать удалить пользователя, у которого они есть.
  private readonly orders = rxResource({
    stream: () => this.orderService.getAll(),
    defaultValue: []
  });

  protected readonly loadError = computed(() => getResourceError(this.users, this.orders));

  protected readonly rows = computed(() =>
    this.users.hasValue()
      ? [...this.users.value()].sort((a, b) =>
          `${a.lastName} ${a.firstName}`.localeCompare(`${b.lastName} ${b.firstName}`, 'ru')
        )
      : []
  );

  protected readonly orderCounts = computed(() => {
    const counts = new Map<string, number>();

    for (const order of this.orders.hasValue() ? this.orders.value() : []) {
      counts.set(order.userId, (counts.get(order.userId) ?? 0) + 1);
    }

    return counts;
  });

  protected readonly fieldError = getFieldError;

  protected initials(user: User): string {
    return `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`.toUpperCase();
  }

  // ----- Создание и изменение -----

  protected readonly isEditorOpen = signal(false);
  protected readonly editedUser = signal<User | null>(null);
  protected readonly isSaving = signal(false);
  protected readonly saveError = signal<string | null>(null);

  protected readonly form = this.formBuilder.group({
    firstName: ['', [notBlank, Validators.maxLength(100)]],
    lastName: ['', [notBlank, Validators.maxLength(100)]],
    email: ['', [notBlank, Validators.email, Validators.maxLength(320)]],
    passwordHash: ['', [notBlank]]
  });

  protected openCreate(): void {
    this.editedUser.set(null);
    this.form.reset();
    this.form.controls.passwordHash.enable();
    this.openEditor();
  }

  protected openEdit(user: User): void {
    this.editedUser.set(user);
    this.form.reset({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email
    });
    // UpdateUserRequest не содержит пароля: при изменении поле не участвует в валидации.
    this.form.controls.passwordHash.disable();
    this.openEditor();
  }

  protected closeEditor(): void {
    this.isEditorOpen.set(false);
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const request = {
      firstName: value.firstName.trim(),
      lastName: value.lastName.trim(),
      email: value.email.trim()
    };

    const user = this.editedUser();

    // В БД уникальный индекс по email: без этой проверки API ответит 500.
    const duplicate = this.rows().find(
      (other) => other.id !== user?.id && other.email.toLowerCase() === request.email.toLowerCase()
    );

    if (duplicate) {
      this.saveError.set(`Пользователь с email ${request.email} уже существует.`);
      return;
    }

    const save$: Observable<unknown> = user
      ? this.userService.update(user.id, request)
      : this.userService.create({ ...request, passwordHash: value.passwordHash });

    this.isSaving.set(true);
    this.saveError.set(null);

    save$.subscribe({
      next: () => {
        this.isSaving.set(false);
        this.isEditorOpen.set(false);
        this.users.reload();
      },
      error: (error: unknown) => {
        this.isSaving.set(false);
        this.saveError.set(getErrorMessage(error));
      }
    });
  }

  private openEditor(): void {
    this.saveError.set(null);
    this.isEditorOpen.set(true);
  }

  // ----- Удаление -----

  protected readonly userToDelete = signal<User | null>(null);
  protected readonly isDeleting = signal(false);
  protected readonly deleteError = signal<string | null>(null);

  protected readonly deleteBlockedReason = computed(() => {
    const user = this.userToDelete();
    const orderCount = user ? (this.orderCounts().get(user.id) ?? 0) : 0;

    return orderCount > 0
      ? `Заказов у пользователя: ${orderCount}. Пока они существуют, сервер не даст удалить пользователя.`
      : null;
  });

  protected askDelete(user: User): void {
    this.deleteError.set(null);
    this.userToDelete.set(user);
  }

  protected cancelDelete(): void {
    this.userToDelete.set(null);
  }

  protected confirmDelete(): void {
    const user = this.userToDelete();

    if (!user) {
      return;
    }

    this.isDeleting.set(true);
    this.deleteError.set(null);

    this.userService.delete(user.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.userToDelete.set(null);
        this.users.reload();
      },
      error: (error: unknown) => {
        this.isDeleting.set(false);
        this.deleteError.set(getErrorMessage(error));
      }
    });
  }

  protected reload(): void {
    this.users.reload();
    this.orders.reload();
  }
}
