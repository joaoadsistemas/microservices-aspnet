import { ICartDetailModel } from "../cart/ICartDetailModel";

export interface IPlaceOrder {
  userId: string;
  couponCode: string;
  purchaseAmount: number;
  discountAmount: number;
  firstName: string;
  lastName: string;
  dateTime: Date;
  email: string;
  phone: string;
  cardNumber: string;
  expirationDate: string;
  securityCode: string;

}
