import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ICartModel } from '../models/cart/ICartModel';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.cartApi;

  constructor(private http: HttpClient) {}

  findCart() {
    return this.http.get<ICartModel>(`${this.baseUrl}find-cart`).pipe(
      map((res: ICartModel) => res)
    );
  }

  addCart(cartModel: ICartModel) {
    return this.http
      .post<ICartModel>(`${this.baseUrl}add-cart`, cartModel)
      .pipe(map((res: ICartModel) => res));
  }

  updateCart(cartModel: ICartModel) {
    return this.http
      .put<ICartModel>(`${this.baseUrl}update-cart`, cartModel)
      .pipe(map((res: ICartModel) => res));
  }

  removeCart(id: number) {
    return this.http.delete<boolean>(`${this.baseUrl}remove-cart/${id}`).pipe(
      map((res: boolean) => res)
    );
  }

  applyCoupon(cartModel: ICartModel) {
    return this.http
      .post<ICartModel>(`${this.baseUrl}apply-coupon`, cartModel)
      .pipe(map((res: ICartModel) => res));
  }

  removeCoupon() {
    return this.http.delete<boolean>(`${this.baseUrl}remove-coupon`).pipe(
      map((res: boolean) => res)
    );
  }
}
