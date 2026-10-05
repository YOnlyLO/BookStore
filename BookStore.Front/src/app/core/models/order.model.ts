// Значения совпадают с BookStore.Core.Enums.OrderStatus: API сериализует enum числом.
export const OrderStatus = {
  New: 0,
  Confirmed: 1,
  Completed: 2,
  Cancelled: 3
} as const;

export type OrderStatus = (typeof OrderStatus)[keyof typeof OrderStatus];

export const ORDER_STATUS_LABELS: Record<OrderStatus, string> = {
  [OrderStatus.New]: 'Новый',
  [OrderStatus.Confirmed]: 'Подтверждён',
  [OrderStatus.Completed]: 'Завершён',
  [OrderStatus.Cancelled]: 'Отменён'
};

export interface OrderItem {
  id: string;
  bookId: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Order {
  id: string;
  userId: string;
  createdAt: string;
  status: OrderStatus;
  items: OrderItem[];
  totalPrice: number;
}

export interface CreateOrderRequest {
  userId: string;
}

export interface AddOrderItemRequest {
  bookId: string;
  quantity: number;
  unitPrice: number;
}
