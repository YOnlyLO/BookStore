import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { Book, CreateBookRequest, UpdateBookRequest } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/books`;

  getAll() {
    return this.http.get<Book[]>(this.apiUrl);
  }

  getById(id: string) {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateBookRequest) {
    return this.http.post<Book>(this.apiUrl, request);
  }

  update(id: string, request: UpdateBookRequest) {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
