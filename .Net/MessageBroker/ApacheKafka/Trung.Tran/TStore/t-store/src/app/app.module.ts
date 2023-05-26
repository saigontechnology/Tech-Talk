import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';

import { NZ_I18N, en_US } from 'ng-zorro-antd/i18n';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { NzTypographyModule } from "ng-zorro-antd/typography";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzDividerModule } from 'ng-zorro-antd/divider';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { ProductTableComponent } from './components/product-table/product-table.component';
import { OrderTableComponent } from './components/order-table/order-table.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';


registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    ProductTableComponent,
    OrderTableComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    NzLayoutModule,
    NzIconModule,
    NzTypographyModule,
    NzTableModule,
    NzDropDownModule,
    NzButtonModule,
    NzFormModule,
    NzInputModule,
    NzGridModule,
    NzMenuModule,
    NzCheckboxModule,
    NzCardModule,
    NzSpaceModule,
    NzSpinModule,
    NzMessageModule,
    NzStatisticModule,
    NzTabsModule,
    NzDividerModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
