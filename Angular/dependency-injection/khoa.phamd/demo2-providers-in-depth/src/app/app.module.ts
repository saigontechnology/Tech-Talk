import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { ProductModule } from '../product/product.module';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  imports: [BrowserModule, FormsModule, AppRoutingModule, ProductModule],
  declarations: [AppComponent],
  bootstrap: [AppComponent],
})
export class AppModule {}
