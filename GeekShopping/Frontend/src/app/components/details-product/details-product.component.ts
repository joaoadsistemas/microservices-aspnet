import { ICartHeaderModel } from './../../models/cart/ICartHeaderModel';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IProductModel } from '../../models/product/IProductModel';
import { CartService } from '../../services/cart.service';
import { IUserToken } from '../../models/auth/IUserToken';
import { ICartDetailModel } from '../../models/cart/ICartDetailModel';
import { ICartModel } from '../../models/cart/ICartModel';

@Component({
  selector: 'app-details-product',
  templateUrl: './details-product.component.html',
  styleUrls: ['./details-product.component.scss'],
})
export class DetailsProductComponent implements OnInit {
  product?: IProductModel;
  productCount: number = 1;
  userLogged: IUserToken | null = null;

  cart?: ICartModel | null;
  cartHeader?: ICartHeaderModel | null;
  cartDetail?: ICartDetailModel | null;
  cartDetails: Array<ICartDetailModel> = [];

  constructor(
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute,
    private cartService: CartService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const productId = Number(params.get('id'));
      if (productId) {
        // Fetch product details using the id
        this.productService.getProductById(productId).subscribe({
          next: (product) => {
            this.product = product;
          },
          error: (error) => {
            console.error('Error fetching product:', error);
          },
        });
      }
    });

    this.authService.userLoggedToken$.subscribe({
      next: (userLogged) => {
        this.userLogged = userLogged;
      },
    });
  }

  back() {
    this.router.navigate(['']);
  }

  addToCart() {
    if (this.userLogged && this.product && this.product.id) {
      this.cart = {
        cartHeader: { id: 0, userId: this.userLogged.id, couponCode: '' },
        cartDetails: [
          {
            id: 0,
            cartHeaderId: 0,
            cartHeader: { id: 0, userId: this.userLogged.id, couponCode: '' },
            productId: this.product.id,
            product: this.product,
            count: this.productCount,
          },
        ],
      };

      this.cartService.addCart(this.cart!).subscribe({
        next: (res) => {
          console.log(this.cart)
          this.router.navigate(['']);
        },
        error: (error) => {
          console.error('Error adding to cart:', error);
        },
      });
    } else {
      console.error('User is not logged in or product is not available');
    }
  }

  replaceLineBreaks(text: string): string {
    return text.replace(/(?:\r\n|\r|\n)/g, '<br>');
  }
}
