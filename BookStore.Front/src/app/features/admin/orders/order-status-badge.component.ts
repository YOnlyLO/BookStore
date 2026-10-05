import { Component, computed, input } from '@angular/core';

import { ORDER_STATUS_LABELS, OrderStatus } from '../../../core/models/order.model';

const STATUS_BADGE_CLASSES: Record<OrderStatus, string> = {
  [OrderStatus.New]: 'badge-info',
  [OrderStatus.Confirmed]: 'badge-accent',
  [OrderStatus.Completed]: 'badge-success',
  [OrderStatus.Cancelled]: ''
};

@Component({
  selector: 'app-order-status-badge',
  template: `<span [class]="classes()">{{ label() }}</span>`
})
export class OrderStatusBadgeComponent {
  readonly status = input.required<OrderStatus>();

  protected readonly label = computed(() => ORDER_STATUS_LABELS[this.status()]);

  protected readonly classes = computed(
    () => `badge badge-dot ${STATUS_BADGE_CLASSES[this.status()]}`
  );
}
