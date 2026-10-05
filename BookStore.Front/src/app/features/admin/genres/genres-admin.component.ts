import { Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { getErrorMessage, getResourceError } from '../../../core/http/api-error';
import { Genre } from '../../../core/models/genre.model';
import { BookService } from '../../../core/services/book.service';
import { GenreService } from '../../../core/services/genre.service';
import { getFieldError, notBlank } from '../../../shared/forms/form-validation';
import { ConfirmDialogComponent } from '../../../shared/ui/confirm-dialog/confirm-dialog.component';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ModalComponent } from '../../../shared/ui/modal/modal.component';

@Component({
  selector: 'app-genres-admin',
  imports: [ReactiveFormsModule, IconComponent, ModalComponent, ConfirmDialogComponent],
  templateUrl: './genres-admin.component.html',
  host: { class: 'page' }
})
export class GenresAdminComponent {
  private readonly genreService = inject(GenreService);
  private readonly bookService = inject(BookService);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly genres = rxResource({
    stream: () => this.genreService.getAll(),
    defaultValue: []
  });

  // Книги нужны, чтобы показать их количество по жанрам и не дать удалить жанр, на который они ссылаются.
  private readonly books = rxResource({
    stream: () => this.bookService.getAll(),
    defaultValue: []
  });

  protected readonly loadError = computed(() => getResourceError(this.genres, this.books));

  protected readonly rows = computed(() =>
    this.genres.hasValue()
      ? [...this.genres.value()].sort((a, b) => a.name.localeCompare(b.name, 'ru'))
      : []
  );

  protected readonly bookCounts = computed(() => {
    const counts = new Map<string, number>();

    for (const book of this.books.hasValue() ? this.books.value() : []) {
      counts.set(book.genreId, (counts.get(book.genreId) ?? 0) + 1);
    }

    return counts;
  });

  protected readonly fieldError = getFieldError;

  // ----- Создание и изменение -----

  protected readonly isEditorOpen = signal(false);
  protected readonly editedGenre = signal<Genre | null>(null);
  protected readonly isSaving = signal(false);
  protected readonly saveError = signal<string | null>(null);

  protected readonly form = this.formBuilder.group({
    name: ['', [notBlank, Validators.maxLength(200)]]
  });

  protected openCreate(): void {
    this.editedGenre.set(null);
    this.form.reset();
    this.openEditor();
  }

  protected openEdit(genre: Genre): void {
    this.editedGenre.set(genre);
    this.form.reset({ name: genre.name });
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

    const genre = this.editedGenre();
    const request = { name: this.form.getRawValue().name.trim() };

    // В БД уникальный индекс по названию: без этой проверки API ответит 500.
    const duplicate = this.rows().find(
      (other) => other.id !== genre?.id && other.name === request.name
    );

    if (duplicate) {
      this.saveError.set(`Жанр «${request.name}» уже существует.`);
      return;
    }

    const save$: Observable<unknown> = genre
      ? this.genreService.update(genre.id, request)
      : this.genreService.create(request);

    this.isSaving.set(true);
    this.saveError.set(null);

    save$.subscribe({
      next: () => {
        this.isSaving.set(false);
        this.isEditorOpen.set(false);
        this.genres.reload();
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

  protected readonly genreToDelete = signal<Genre | null>(null);
  protected readonly isDeleting = signal(false);
  protected readonly deleteError = signal<string | null>(null);

  protected readonly deleteBlockedReason = computed(() => {
    const genre = this.genreToDelete();
    const bookCount = genre ? (this.bookCounts().get(genre.id) ?? 0) : 0;

    return bookCount > 0
      ? `Книг в этом жанре: ${bookCount}. Перенесите их в другой жанр или удалите — ` +
          'пока на жанр ссылаются книги, сервер не даст его удалить.'
      : null;
  });

  protected askDelete(genre: Genre): void {
    this.deleteError.set(null);
    this.genreToDelete.set(genre);
  }

  protected cancelDelete(): void {
    this.genreToDelete.set(null);
  }

  protected confirmDelete(): void {
    const genre = this.genreToDelete();

    if (!genre) {
      return;
    }

    this.isDeleting.set(true);
    this.deleteError.set(null);

    this.genreService.delete(genre.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.genreToDelete.set(null);
        this.genres.reload();
      },
      error: (error: unknown) => {
        this.isDeleting.set(false);
        this.deleteError.set(getErrorMessage(error));
      }
    });
  }

  protected reload(): void {
    this.genres.reload();
    this.books.reload();
  }
}
