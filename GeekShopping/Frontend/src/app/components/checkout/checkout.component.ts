import { CouponService } from './../../services/coupon.service';
import { CartService } from './../../services/cart.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ICartHeaderModel } from '../../models/cart/ICartHeaderModel';
import { ICartModel } from '../../models/cart/ICartModel';
import { IPlaceOrder } from '../../models/placeorder/IPlaceOrder';
import { ICouponModel } from '../../models/coupon/ICouponModel';
import { HttpErrorResponse } from '@angular/common/http';

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
  err412: boolean = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cartService: CartService,
    private couponService: CouponService
  ) {
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
    // seta o erro 412 para false sempre que tiver uma requisição de compra
    this.err412 = false;

    if (this.placeOrderForm.valid) {
      console.log('Order placed', this.placeOrderForm.value);
      this.placeOrderModel = this.placeOrderForm.value;

      if (this.discount && this.coupon) {
        this.placeOrderModel!.couponCode = this.coupon.couponCode;
        this.placeOrderModel!.discountAmount = this.discount;
      } else {
        this.placeOrderModel!.couponCode = '';
        this.placeOrderModel!.discountAmount = 0;
      }

      this.placeOrderModel!.purchaseAmount = this.total;
      this.placeOrderModel!.userId = this.cartHeader.userId;
      this.placeOrderModel!.dateTime = new Date();

      console.log('Place order model:', this.placeOrderModel);

      this.cartService.checkout(this.placeOrderModel!).subscribe({
        next: (res) => {
          console.log('Order placed:', res);
          this.router.navigate(['order-confirmation']);
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 412) {
            console.error('Precondition Failed:', error.message);
            // Aqui você pode adicionar uma lógica específica para lidar com o erro 412
            this.err412 = true;

            // PEGA O NOVO VALOR ATRIBUIDO AO CUPOM E ATUALIZA A VARIAVEL
            this.couponService
              .getCoupon(this.placeOrderModel!.couponCode)
              .subscribe({
                next: (res) => {
                  console.log('Coupon found:', res);
                  this.coupon = res;
                  this.discount = this.coupon.discountAmount;
                },
                error: (error) => {
                  console.error('Error finding coupon:', error);
                  alert(
                    'An error occurred while finding the coupon. Please try again later.'
                  );
                },
              });
          } else {
            console.error('Error placing order:', error);
            alert(
              'An error occurred while placing your order. Please try again later.'
            );
          }
        },
      });
    } else {
      console.log('Form is invalid');
    }
  }
}
