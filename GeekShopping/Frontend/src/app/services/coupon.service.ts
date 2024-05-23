import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Injectable } from '@angular/core';
import { ICouponModel } from '../models/coupon/ICouponModel';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CouponService {
  baseUrl = environment.couponApi;

  constructor(private http: HttpClient) {}

  getCoupon(couponCode: string) {
    return this.http
      .get<ICouponModel>(`${this.baseUrl}${couponCode}`)
      .pipe(map((res: ICouponModel) => res));
  }
}
