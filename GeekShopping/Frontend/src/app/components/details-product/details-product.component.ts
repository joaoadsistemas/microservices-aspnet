import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IProductModel } from '../../models/product/IProductModel';

@Component({
  selector: 'app-details-product',
  templateUrl: './details-product.component.html',
  styleUrls: ['./details-product.component.scss'],
})
export class DetailsProductComponent implements OnInit {
  product?: IProductModel;
  productCount: number = 1;

  constructor(
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
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
  }

  back() {
    this.router.navigate(['']);
  }

  replaceLineBreaks(text: string): string {
    return text.replace(/(?:\r\n|\r|\n)/g, '<br>');
  }
}
