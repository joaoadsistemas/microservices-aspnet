import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ICartHeaderModel } from '../../models/cart/ICartHeaderModel';
import { ICartModel } from '../../models/cart/ICartModel';
import { IPlaceOrder } from '../../models/placeorder/IPlaceOrder';
import { ICouponModel } from '../../models/coupon/ICouponModel';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnInit {
  placeOrderForm: FormGroup;
  placeOrderModel?: IPlaceOrder;
  cart: ICartModel;
  total: number;
  discount: number;
  coupon: ICouponModel;
  cartHeader: ICartHeaderModel;

  constructor(private router: Router, private fb: FormBuilder) {
    this.placeOrderForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [
        '',
        [
          Validators.required,
          Validators.minLength(11),
          Validators.maxLength(11),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
      cardNumber: [
        '',
        [
          Validators.required,
          Validators.minLength(16),
          Validators.maxLength(16),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
      expirationDate: [
        '',
        [Validators.required, Validators.pattern('^(0[1-9]|1[0-2])/[0-9]{2}$')],
      ],
      securityCode: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(3),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
      userId: [''],
      id: [''],
      couponCode: [''],
      discountAmount: [''],
      purchaseAmount: [''],
    });

    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as any;
    this.cart = state.cart;
    this.total = state.total;
    this.discount = state.discount;
    this.coupon = state.coupon;
    this.cartHeader = state.cart.cartHeader;
  }

  ngOnInit(): void {
    console.log(this.cart); // Use this.cart in your template or logic as needed
  }

  placeOrder(): void {
    if (this.placeOrderForm.valid) {
      // Process the order securely, e.g., send the data to a secure backend service
      console.log('Order placed', this.placeOrderForm.value);
      this.placeOrderModel = this.placeOrderForm.value;
      this.placeOrderModel!.couponCode = this.coupon.couponCode;
      this.placeOrderModel!.discountAmount = this.discount;
      this.placeOrderModel!.purchaseAmount = this.total;
      this.placeOrderModel!.userId = this.cartHeader.userId;
      this.placeOrderModel!.dateTime = new Date();
      // ESTA DANDO CERTO!!!!!!!!!! CONTINUE DAQUI JOÃO
      //AGORA É SO PASSAR ESSE OBJETO PARA O BACKEND, OS DADOS JA ESTAO PRONTOS
      console.log(this.placeOrderModel);
    } else {
      console.log('Form is invalid');
    }
  }
}
