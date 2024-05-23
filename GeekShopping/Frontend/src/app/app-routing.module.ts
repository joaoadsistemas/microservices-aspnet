import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './pages/product/product.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { DetailsProductComponent } from './components/details-product/details-product.component';
import { CartComponent } from './components/cart/cart.component';

const routes: Routes = [
  { path: 'product', component: ProductComponent },
  { path: 'product-card/:id', component: ProductCardComponent },
  { path: 'product-details/:id', component: DetailsProductComponent },
  { path: 'cart', component: CartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
