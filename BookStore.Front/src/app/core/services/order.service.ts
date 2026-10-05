import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { AddOrderItemRequest, CreateOrderRequest, Order } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/orders`;

  getAll() {
    return this.http.get<Order[]>(this.apiUrl);
  }

  getById(id: string) {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateOrderRequest) {
    return this.http.post<Order>(this.apiUrl, request);
  }

  addItem(orderId: string, request: AddOrderItemRequest) {
    return this.http.post<void>(`${this.apiUrl}/${orderId}/items`, request);
  }

  removeItem(orderId: string, bookId: string) {
    return this.http.delete<void>(`${this.apiUrl}/${orderId}/items/${bookId}`);
  }

  confirm(orderId: string) {
    return this.http.post<void>(`${this.apiUrl}/${orderId}/confirm`, null);
  }

  complete(orderId: string) {
    return this.http.post<void>(`${this.apiUrl}/${orderId}/complete`, null);
  }

  cancel(orderId: string) {
    return this.http.post<void>(`${this.apiUrl}/${orderId}/cancel`, null);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
