import { Component, inject } from '@angular/core';
import { RouterLink, Router } from '@angular/router'; // Додайте Router
import { AsyncPipe, CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule, AsyncPipe],
  templateUrl: './header.html'
})
export class Header {
  private authService = inject(AuthService);
  private router = inject(Router); // Додано Router для навігації

  isLoggedIn$: Observable<boolean> = this.authService.isLoggedIn$;
  currentUserEmail$: Observable<string> = this.authService.currentUserEmail$;
  currentUsername$: Observable<string> = this.authService.currentUsername$;

  constructor() { }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/events']);
  }
}