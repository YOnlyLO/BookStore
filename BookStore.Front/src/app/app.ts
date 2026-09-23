import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GenresComponent } from './features/genres/genres.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    GenresComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}