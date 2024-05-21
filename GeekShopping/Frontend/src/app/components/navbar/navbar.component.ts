import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  userLogged: boolean = false;

  constructor(private authService: AuthService) {
    this.authService.userLoggedToken$.subscribe({
      next: (userLogged) => {
        this.userLogged = userLogged ? true : false;
      },
    });
  }

  logout() {
    this.authService.logout();
  }
}
