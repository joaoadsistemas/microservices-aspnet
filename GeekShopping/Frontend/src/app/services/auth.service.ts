import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IUserToken } from '../models/auth/IUserToken';
import { ILoginDTO } from '../models/auth/ILoginDTO';
import { IRegisterDTO } from '../models/auth/IRegisterDTO';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  baseUrl: string = environment.authApi;

  private userLogged = new BehaviorSubject<IUserToken | null>(null);
  userLoggedToken$ = this.userLogged.asObservable();

  setUserLogged(userToken: IUserToken) {
    this.userLogged.next(userToken);
  }

  logout() {
    this.userLogged.next(null);
    localStorage.clear();
  }

  login(loginBody: ILoginDTO) {
    return this.http.post<any>(this.baseUrl + 'login', loginBody).pipe(
      map((response: IUserToken) => {
        if (response) {
          localStorage.setItem('user', JSON.stringify(response));
          this.setUserLogged(response);
        }
        return response;
      })
    );
  }

  registerClient(user: IRegisterDTO) {
    return this.http.post<any>(this.baseUrl + 'register/client', user).pipe(
      map((response) => {
        return response;
      })
    );
  }
}
