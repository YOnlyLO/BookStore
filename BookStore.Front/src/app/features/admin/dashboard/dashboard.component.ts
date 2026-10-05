import { Component, computed, inject, ResourceRef } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';

import { getResourceError } from '../../../core/http/api-error';
import { OrderStatus } from '../../../core/models/order.model';
import { BookService } from '../../../core/services/book.service';
import { GenreService } from '../../../core/services/genre.service';
import { OrderService } from '../../../core/services/order.service';
import { UserService } from '../../../core/services/user.service';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ADMIN_SECTIONS, AdminSectionPath } from '../admin-sections';
import { OrderStatusBadgeComponent } from '../orders/order-status-badge.component';

function countOf<T>(resource: ResourceRef<T[] | undefined>): number | null {
  return resource.hasValue() ? (resource.value()?.length ?? 0) : null;
}

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, IconComponent, OrderStatusBadgeComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  host: { class: 'page' }
})
export class DashboardComponent {
  private readonly genreService = inject(GenreService);
  private readonly bookService = inject(BookService);
  private readonly userService = inject(UserService);
  private readonly orderService = inject(OrderService);

  private readonly genres = rxResource({ stream: () => this.genreService.getAll() });
  private readonly books = rxResource({ stream: () => this.bookService.getAll() });
  private readonly users = rxResource({ stream: () => this.userService.getAll() });
  private readonly orders = rxResource({ stream: () => this.orderService.getAll() });

  protected readonly loadError = computed(() =>
    getResourceError(this.genres, this.books, this.users, this.orders)
  );

  protected readonly cards = computed(() => {
    const counts: Record<AdminSectionPath, number | null> = {
      genres: countOf(this.genres),
      books: countOf(this.books),
      users: countOf(this.users),
      orders: countOf(this.orders)
    };

    return ADMIN_SECTIONS.map((section) => ({ ...section, count: counts[section.path] }));
  });

  protected readonly orderStats = computed(() => {
    if (!this.orders.hasValue()) {
      return null;
    }

    const orders = this.orders.value() ?? [];

    return Object.values(OrderStatus).map((status) => ({
      status,
      count: orders.filter((order) => order.status === status).length
    }));
  });

  protected reload(): void {
    this.genres.reload();
    this.books.reload();
    this.users.reload();
    this.orders.reload();
  }
}
