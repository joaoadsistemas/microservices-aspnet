import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  constructor(private authService: AuthService) {}
  setUserLogged() {
    const userString = localStorage.getItem('user');
    if (!userString) {
      return;
    }

    const userToken = JSON.parse(userString);
    this.authService.setUserLogged(userToken);
  }

  ngOnInit(): void {
    this.setUserLogged();
  }

  title = 'frontend';
}
