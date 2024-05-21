import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ILoginDTO } from '../../models/auth/ILoginDTO';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm?.valid) {
      const loginData: ILoginDTO = this.loginForm.value;
      this.authService.login(loginData).subscribe({
        next: (response) => {
          this.router.navigateByUrl('');
        },
        error: (error) => {
          console.error('error:', error);
        },
      });
    }
  }
}
