import { Component, OnInit } from '@angular/core';
import { IUserToken } from '../../models/auth/IUserToken';
import { ICartModel } from '../../models/cart/ICartModel';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent implements OnInit {
  cart: ICartModel | null = null;
  userLogged: IUserToken | null = null;
  couponCode: string = '';

  constructor(
    private cartService: CartService,
    private authService: AuthService
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
      },
      error: (error) => {
        console.error('Error fetching cart:', error);
      },
    });
  }

  applyCoupon(): void {}

  removeCartDetail(cartDetailId: number): void {
    this.cartService.removeCart(cartDetailId).subscribe({
      next: (res: any) => {
        if (res) {
          this.cart = this.cart
            ? {
                ...this.cart,
                cartDetails: this.cart.cartDetails.filter(
                  (detail) => detail.id !== cartDetailId
                ),
              }
            : null;
        }
      },
      error: (error) => {
        console.error('Error removing cart detail:', error);
      },
    });
  }

  checkout(): void {}

  get totalPrice(): number {
    return this.cart
      ? this.cart.cartDetails.reduce(
          (total, detail) => total + detail.product.price * detail.count,
          0
        )
      : 0;
  }
}
