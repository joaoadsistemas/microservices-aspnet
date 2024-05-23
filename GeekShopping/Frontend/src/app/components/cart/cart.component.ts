import { Component, OnInit } from '@angular/core';
import { IUserToken } from '../../models/auth/IUserToken';
import { ICartModel } from '../../models/cart/ICartModel';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';
import { CouponService } from '../../services/coupon.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent implements OnInit {
  cart: ICartModel | null = null;
  userLogged: IUserToken | null = null;
  couponCode: string = '';
  couponActive: boolean = false;
  errmsg: boolean = false;
  discount: number = 0; // Variable to store the discount amount
  total: number = 0;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private couponService: CouponService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.userLoggedToken$.subscribe((user) => {
      this.userLogged = user;
      if (user) {
        this.loadCart(user.id);
      }
    });
  }

  loadCart(userId: string): void {
    this.cartService.findCart().subscribe({
      next: (cart) => {
        this.cart = cart;
        this.calculateTotal();
      },
      error: (error) => {
        console.error('Error fetching cart:', error);
      },
    });
  }

  calculateTotal(): void {
    this.total = 0;
    if (this.cart) {
      this.cart.cartDetails.forEach((cd) => {
        this.total += cd.product.price * cd.count;
      });
      if (this.couponActive) {
        this.total -= this.discount;
      }
    }
  }

  removeCartDetail(cartDetailId: number): void {
    this.cartService.removeCart(cartDetailId).subscribe({
      next: (res: any) => {
        if (res && this.cart) {
          this.cart.cartDetails = this.cart.cartDetails.filter(
            (detail) => detail.id !== cartDetailId
          );
          this.calculateTotal();
        }
      },
      error: (error) => {
        console.error('Error removing cart detail:', error);
      },
    });
  }

  applyCoupon(): void {
    if (this.couponCode) {
      this.couponService.getCoupon(this.couponCode).subscribe({
        next: (coupon) => {
          if (coupon && this.cart) {
            this.cart.cartHeader.couponCode = coupon.couponCode;

            this.cart.cartDetails.forEach((cd) => {
              cd.cartHeader = this.cart!.cartHeader;
            });

            this.cartService.applyCoupon(this.cart).subscribe({
              next: (res) => {
                this.discount = coupon.discountAmount;
                this.couponActive = true;
                this.calculateTotal();
              },
              error: (error) => {
                console.error('Error applying coupon:', error);
              },
            });
          } else {
            this.errmsg = true;
          }
        },
        error: (error) => {
          console.error('Error fetching coupon:', error);
          this.errmsg = true;
        },
      });
    }
  }

  removeCoupon(): void {
    if (this.cart) {
      this.cartService.removeCoupon().subscribe({
        next: (res) => {
          if (res) {
            this.cart!.cartHeader.couponCode = '';
            this.couponActive = false;
            this.discount = 0;
            this.calculateTotal();
          }
        },
        error: (error) => {
          console.error('Error removing coupon:', error);
        },
      });
    }
  }

  checkout(): void {
    this.router.navigate(['checkout'], {
      state: { cart: this.cart, total: this.total, discount: this.discount },
    });
  }
}
