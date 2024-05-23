import { ICartDetailModel } from './ICartDetailModel';
import { ICartHeaderModel } from './ICartHeaderModel';

export interface ICartModel {
  cartHeader: ICartHeaderModel;
  cartDetails: ICartDetailModel[];
}
