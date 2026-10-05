import { afterNextRender, Component, ElementRef, input, output, viewChild } from '@angular/core';

import { IconComponent } from '../icon/icon.component';

let nextModalId = 0;

/**
 * Модальное окно на нативном <dialog>: фокус, Esc и затемнение фона даёт браузер.
 * Показывается, пока присутствует в шаблоне родителя (оборачивайте в @if).
 * Содержимое проецируется целиком — используйте глобальные классы .modal-body и .modal-footer.
 */
@Component({
  selector: 'app-modal',
  imports: [IconComponent],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css'
})
export class ModalComponent {
  readonly title = input.required<string>();
  readonly size = input<'md' | 'lg'>('md');

  readonly closed = output<void>();

  // Модальные окна бывают вложенными, поэтому id заголовка уникален для каждого экземпляра.
  protected readonly titleId = `modal-title-${nextModalId++}`;

  private readonly dialog = viewChild.required<ElementRef<HTMLDialogElement>>('dialog');

  constructor() {
    afterNextRender(() => this.dialog().nativeElement.showModal());
  }

  protected onCancel(event: Event): void {
    // Закрытием управляет родитель: он уберёт модальное окно из шаблона.
    event.preventDefault();
    this.closed.emit();
  }
}
