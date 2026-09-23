import { Component, inject, signal } from '@angular/core';
import { Genre } from '../../core/models/genre.model';
import { GenreService } from '../../core/services/genre.service';

@Component({
  selector: 'app-genres',
  standalone: true,
  templateUrl: './genres.component.html',
  styleUrl: './genres.component.css'
})
export class GenresComponent {
  private readonly genreService = inject(GenreService);

  protected readonly genres = signal<Genre[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly error = signal<string | null>(null);

  constructor() {
    this.loadGenres();
  }

  private loadGenres(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.genreService.getAll().subscribe({
      next: (genres) => {
        this.genres.set(genres);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Не удалось загрузить жанры.');
        this.isLoading.set(false);
      }
    });
  }
}
