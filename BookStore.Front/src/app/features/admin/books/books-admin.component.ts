import { CurrencyPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { getErrorMessage, getResourceError } from '../../../core/http/api-error';
import { Book } from '../../../core/models/book.model';
import { BookService } from '../../../core/services/book.service';
import { GenreService } from '../../../core/services/genre.service';
import { OrderService } from '../../../core/services/order.service';
import { getFieldError, integer, notBlank } from '../../../shared/forms/form-validation';
import { ConfirmDialogComponent } from '../../../shared/ui/confirm-dialog/confirm-dialog.component';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ModalComponent } from '../../../shared/ui/modal/modal.component';

@Component({
  selector: 'app-books-admin',
  imports: [CurrencyPipe, ReactiveFormsModule, IconComponent, ModalComponent, ConfirmDialogComponent],
  templateUrl: './books-admin.component.html',
  host: { class: 'page' }
})
export class BooksAdminComponent {
  private readonly bookService = inject(BookService);
  private readonly genreService = inject(GenreService);
  private readonly orderService = inject(OrderService);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly books = rxResource({
    stream: () => this.bookService.getAll(),
    defaultValue: []
  });

  private readonly genresResource = rxResource({
    stream: () => this.genreService.getAll(),
    defaultValue: []
  });

  // Заказы нужны только для проверки: книгу, которая есть в заказах, сервер удалить не даст.
  private readonly orders = rxResource({
    stream: () => this.orderService.getAll(),
    defaultValue: []
  });

  protected readonly loadError = computed(() =>
    getResourceError(this.books, this.genresResource, this.orders)
  );

  protected readonly rows = computed(() =>
    this.books.hasValue()
      ? [...this.books.value()].sort((a, b) => a.title.localeCompare(b.title, 'ru'))
      : []
  );

  protected readonly genres = computed(() =>
    this.genresResource.hasValue()
      ? [...this.genresResource.value()].sort((a, b) => a.name.localeCompare(b.name, 'ru'))
      : []
  );

  protected readonly genreNames = computed(
    () => new Map(this.genres().map((genre) => [genre.id, genre.name]))
  );

  private readonly orderedBookIds = computed(() => {
    const orders = this.orders.hasValue() ? this.orders.value() : [];

    return new Set(orders.flatMap((order) => order.items.map((item) => item.bookId)));
  });

  protected readonly fieldError = getFieldError;

  // ----- Создание и изменение -----

  protected readonly isEditorOpen = signal(false);
  protected readonly editedBook = signal<Book | null>(null);
  protected readonly isSaving = signal(false);
  protected readonly saveError = signal<string | null>(null);

  protected readonly form = this.formBuilder.group({
    title: ['', [notBlank, Validators.maxLength(200)]],
    author: ['', [notBlank, Validators.maxLength(200)]],
    genreId: ['', [Validators.required]],
    price: this.formBuilder.control<number | null>(null, [
      Validators.required,
      Validators.min(0.01)
    ]),
    stockQuantity: this.formBuilder.control<number | null>(0, [
      Validators.required,
      Validators.min(0),
      integer
    ]),
    description: ['', [Validators.maxLength(2000)]]
  });

  protected openCreate(): void {
    this.editedBook.set(null);
    this.form.reset();
    this.form.controls.stockQuantity.enable();
    this.openEditor();
  }

  protected openEdit(book: Book): void {
    this.editedBook.set(book);
    this.form.reset({
      title: book.title,
      author: book.author,
      genreId: book.genreId,
      price: book.price,
      stockQuantity: book.stockQuantity,
      description: book.description ?? ''
    });
    // UpdateBookRequest не содержит остатка — изменить его через API нельзя.
    this.form.controls.stockQuantity.disable();
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
      title: value.title.trim(),
      author: value.author.trim(),
      description: value.description.trim() || null,
      price: value.price ?? 0,
      genreId: value.genreId
    };

    const book = this.editedBook();
    const save$: Observable<unknown> = book
      ? this.bookService.update(book.id, request)
      : this.bookService.create({ ...request, stockQuantity: value.stockQuantity ?? 0 });

    this.isSaving.set(true);
    this.saveError.set(null);

    save$.subscribe({
      next: () => {
        this.isSaving.set(false);
        this.isEditorOpen.set(false);
        this.books.reload();
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

  protected readonly bookToDelete = signal<Book | null>(null);
  protected readonly isDeleting = signal(false);
  protected readonly deleteError = signal<string | null>(null);

  protected readonly deleteBlockedReason = computed(() => {
    const book = this.bookToDelete();

    return book && this.orderedBookIds().has(book.id)
      ? 'Книга есть в заказах — пока на неё ссылаются позиции заказов, сервер не даст её удалить.'
      : null;
  });

  protected askDelete(book: Book): void {
    this.deleteError.set(null);
    this.bookToDelete.set(book);
  }

  protected cancelDelete(): void {
    this.bookToDelete.set(null);
  }

  protected confirmDelete(): void {
    const book = this.bookToDelete();

    if (!book) {
      return;
    }

    this.isDeleting.set(true);
    this.deleteError.set(null);

    this.bookService.delete(book.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.bookToDelete.set(null);
        this.books.reload();
      },
      error: (error: unknown) => {
        this.isDeleting.set(false);
        this.deleteError.set(getErrorMessage(error));
      }
    });
  }

  protected reload(): void {
    this.books.reload();
    this.genresResource.reload();
    this.orders.reload();
  }
}
