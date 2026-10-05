import { Routes } from '@angular/router';

import { AdminLayoutComponent } from './layout/admin-layout.component';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        title: 'Дашборд · BookStore',
        loadComponent: () =>
          import('./dashboard/dashboard.component').then((m) => m.DashboardComponent)
      },
      {
        path: 'genres',
        title: 'Жанры · BookStore',
        loadComponent: () =>
          import('./genres/genres-admin.component').then((m) => m.GenresAdminComponent)
      },
      {
        path: 'books',
        title: 'Книги · BookStore',
        loadComponent: () =>
          import('./books/books-admin.component').then((m) => m.BooksAdminComponent)
      },
      {
        path: 'users',
        title: 'Пользователи · BookStore',
        loadComponent: () =>
          import('./users/users-admin.component').then((m) => m.UsersAdminComponent)
      },
      {
        path: 'orders',
        title: 'Заказы · BookStore',
        loadComponent: () =>
          import('./orders/orders-admin.component').then((m) => m.OrdersAdminComponent)
      }
    ]
  }
];
