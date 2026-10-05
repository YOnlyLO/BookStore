import { IconName } from '../../shared/ui/icon/icon.component';

export type AdminSectionPath = 'genres' | 'books' | 'users' | 'orders';

export interface AdminSection {
  path: AdminSectionPath;
  title: string;
  description: string;
  icon: IconName;
}

/** Разделы админ-панели: общие для бокового меню и дашборда. */
export const ADMIN_SECTIONS: readonly AdminSection[] = [
  {
    path: 'genres',
    title: 'Жанры',
    description: 'Справочник жанров, к которым относятся книги',
    icon: 'tag'
  },
  {
    path: 'books',
    title: 'Книги',
    description: 'Каталог: авторы, цены, остатки на складе',
    icon: 'book'
  },
  {
    path: 'users',
    title: 'Пользователи',
    description: 'Учётные записи покупателей',
    icon: 'users'
  },
  {
    path: 'orders',
    title: 'Заказы',
    description: 'Состав заказов и их статусы',
    icon: 'cart'
  }
];
