import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { Genre } from '../models/genre.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/genres`;

  getAll() {
    return this.http.get<Genre[]>(this.apiUrl);
  }

  getById(id: string) {
    return this.http.get<Genre>(`${this.apiUrl}/${id}`);
  }
}
