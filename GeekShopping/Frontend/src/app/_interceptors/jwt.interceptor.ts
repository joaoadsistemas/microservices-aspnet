import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.authService.userLoggedToken$.pipe(take(1)).subscribe({
      next: (userLogged) => {
        if (userLogged) {
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ${userLogged.token}`,
            },
          });
        }
      },
    });
    return next.handle(req);
  }
}
