import { NgModule } from '@angular/core';
import {
  BrowserModule,
  provideClientHydration,
} from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProductComponent } from './pages/product/product.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DetailsProductComponent } from './components/details-product/details-product.component';
import { CartComponent } from './components/cart/cart.component';
import { CheckoutComponent } from './components/checkout/checkout.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ProductComponent,
    ProductCardComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    DetailsProductComponent,
    CartComponent,
    CheckoutComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [
    provideClientHydration(),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
