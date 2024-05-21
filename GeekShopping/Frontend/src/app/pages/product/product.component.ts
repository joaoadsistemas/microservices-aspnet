import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { IProductModel } from '../../models/product/IProductModel';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss',
})
export class ProductComponent implements OnInit {
  products?: Array<IProductModel>;

  constructor(private productService: ProductService, private router: Router) {}
  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.productService.getProductsAll().subscribe({
      next: (res) => {
        this.products = res;
        console.log(this.products);
      },
    });
  }

  editProduct(product: IProductModel) {
    this.router.navigate(['product-card', product.id]); // Passa apenas o id do produto
  }

  createProduct() {
    this.router.navigate(['product-card', 0]);
  }

  deleteProduct(product: IProductModel) {
    if (product.id) {
      this.productService.deleteProduct(product.id).subscribe({
        next: () => {
          this.getProducts(); // Recarrega a lista de produtos após a exclusão
        },
      });
    }
  }
}
