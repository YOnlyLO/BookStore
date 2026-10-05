import { Component, input, output } from '@angular/core';

import { ModalComponent } from '../modal/modal.component';

@Component({
  selector: 'app-confirm-dialog',
  imports: [ModalComponent],
  templateUrl: './confirm-dialog.component.html'
})
export class ConfirmDialogComponent {
  readonly title = input.required<string>();
  readonly message = input.required<string>();
  readonly confirmLabel = input('Удалить');
  readonly cancelLabel = input('Отмена');
  /** Если задано — действие заведомо невозможно: показываем причину и блокируем кнопку. */
  readonly blockedReason = input<string | null>(null);
  readonly busy = input(false);
  readonly error = input<string | null>(null);

  readonly confirmed = output<void>();
  readonly cancelled = output<void>();
}
