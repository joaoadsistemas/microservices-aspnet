import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IRegisterDTO } from '../../models/auth/IRegisterDTO';
import { Router } from '@angular/router';
import { min } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private AuthService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const registerData: IRegisterDTO = this.registerForm.value;
      this.AuthService.registerClient(registerData).subscribe({
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
