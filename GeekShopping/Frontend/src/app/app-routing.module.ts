import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './pages/product/product.component';
import { ProductCardComponent } from './components/product-card/product-card.component';

const routes: Routes = [
  { path: 'product', component: ProductComponent },
  { path: 'product-card/:id', component: ProductCardComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
