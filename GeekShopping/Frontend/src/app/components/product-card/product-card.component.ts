import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { IProductModel } from '../../models/IProductModel';
import { ActivatedRoute, Router } from '@angular/router';
import { pipe } from 'rxjs';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss',
})
export class ProductCardComponent {
  product: IProductModel | null = null;
  productForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe((params) => {
      const productId = Number.parseInt(params.get('id') || '0');
      if (productId != 0) {
        // Buscar os detalhes do produto usando o id
        this.productService.getProductById(productId).subscribe((product) => {
          this.product = product;
          // Inicializar o formulário com os valores do produto, se existir
          if (this.product?.id != 0) {
            this.productForm.patchValue({
              name: this.product.name,
              categoryName: this.product.categoryName,
              description: this.product.description,
              imageURL: this.product.imageUrl,
              price: this.product.price,
            });
          }
        });
      } else {
        this.product = null;
      }
      console.log('product:', this.product);
    });
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      categoryName: ['', Validators.required],
      description: ['', Validators.required],
      imageURL: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
    });
  }

  onSubmit(): void {
    if (this.product != null && this.productForm.valid) {
      if (this.product?.id) {
        const updatedProduct: IProductModel = {
          ...this.productForm.value,
          id: this.product?.id,
        };
        this.productService
          .updateProduct(updatedProduct, this.product.id)
          .subscribe({
            next: () => {
              this.router.navigate(['/product']);
            },
          });
      }
    }

    const newProduct: IProductModel = this.productForm.value;
    this.productService.addProduct(newProduct).subscribe({
      next: () => {
        this.router.navigate(['/product']);
      },
    });
  }

  onDelete(): void {
    if (this.product) {
      if (this.product.id) {
        this.productService.deleteProduct(this.product.id).subscribe({
          next: () => {
            this.router.navigate(['/product']);
          },
        });
      }
    }
  }

  onBack(): void {
    this.router.navigate(['/product']);
  }
}
