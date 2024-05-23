import { IProductModel } from '../product/IProductModel';
import { ICartHeaderModel } from './ICartHeaderModel';

export interface ICartDetailModel {
  id: number;
  cartHeaderId: number;
  cartHeader: ICartHeaderModel;
  productId: number;
  product: IProductModel;
  count: number;
}
