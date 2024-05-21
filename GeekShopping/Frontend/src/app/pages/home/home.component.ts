import { Component, OnInit } from '@angular/core';
import { IProductModel } from '../../models/product/IProductModel';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  products: Array<IProductModel> = [];

  userLogged: boolean = false;

  constructor(
    private productService: ProductService,
    private router: Router,
    private authService: AuthService
  ) {
    this.authService.userLoggedToken$.subscribe({
      next: (userLogged) => {
        this.userLogged = userLogged ? true : false;
      },
    });
  }

  ngOnInit(): void {
    this.productService.getProductsAll().subscribe({
      next: (response) => {
        this.products = response.map((product) => ({
          ...product,
          name: this.truncateText(product.name, 24),
          description: this.truncateText(product.description, 324),
        }));
      },
      error: (error) => {
        console.error('error:', error);
      },
    });
  }

  truncateText(text: string, maxLength: number): string {
    if (text.length > maxLength) {
      return text.substr(0, maxLength) + '...';
    }
    return text;
  }

  replaceLineBreaks(text: string): string {
    return text.replace(/(?:\r\n|\r|\n)/g, '<br>');
  }

  detailsProduct(product: IProductModel) {
    if (this.userLogged) {
      this.router.navigate(['product-details', product.id]);
    } else {
      this.router.navigate(['login']);
    }
  }
}
