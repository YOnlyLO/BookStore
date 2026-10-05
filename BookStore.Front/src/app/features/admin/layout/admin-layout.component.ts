import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { environment } from '../../../../environments/environment';
import { IconComponent } from '../../../shared/ui/icon/icon.component';
import { ADMIN_SECTIONS } from '../admin-sections';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, IconComponent],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css'
})
export class AdminLayoutComponent {
  protected readonly sections = ADMIN_SECTIONS;
  protected readonly apiUrl = environment.apiUrl;
}
