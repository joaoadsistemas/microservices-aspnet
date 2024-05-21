import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { map } from 'rxjs';
import { IProductModel } from '../models/product/IProductModel';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}
  productApi = environment.productApi;

  getProductsAll() {
    return this.http.get<Array<IProductModel>>(this.productApi).pipe(
      map((res: Array<IProductModel>) => {
        return res;
      })
    );
  }

  getProductById(id: number) {
    return this.http.get<IProductModel>(`${this.productApi}/${id}`).pipe(
      map((res: IProductModel) => {
        return res;
      })
    );
  }

  addProduct(product: IProductModel) {
    return this.http.post<IProductModel>(this.productApi, product).pipe(
      map((res: IProductModel) => {
        return res;
      })
    );
  }

  updateProduct(product: IProductModel, id: number) {
    return this.http
      .put<IProductModel>(`${this.productApi}/${id}`, product)
      .pipe(
        map((res: IProductModel) => {
          return res;
        })
      );
  }

  deleteProduct(id: number) {
    return this.http.delete<any>(`${this.productApi}/${id}`).pipe(
      map((res: any) => {
        return res;
      })
    );
  }
}
