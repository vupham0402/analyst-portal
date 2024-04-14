import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { DashboardComponent } from './features/public/dashboard/dashboard.component';
import { LoginComponent } from './features/auth/login/login.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { RegisterComponent } from './features/auth/register/register.component';
import { ListOrganizationComponent } from './features/organization/list-organization/list-organization/list-organization.component';
import { AddOrganizationComponent } from './features/organization/add-organization/add-organization/add-organization.component';
import { ImageSelectorComponent } from './shared/components/image-selector/image-selector.component';
import { EditOrganizationComponent } from './features/organization/edit-organization/edit-organization/edit-organization.component';
import { AddCategoryComponent } from './features/category/add-category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category/edit-category.component';
import { ListCategoryComponent } from './features/category/list-category/list-category/list-category.component';
import { ListSalesComponent } from './features/sales/list-sales/list-sales.component';
import { AddSalesComponent } from './features/sales/add-sales/add-sales/add-sales.component';
import { EditSalesComponent } from './features/sales/edit-sales/edit-sales/edit-sales.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    DashboardComponent,
    LoginComponent,
    RegisterComponent,
    ListOrganizationComponent,
    AddOrganizationComponent,
    ImageSelectorComponent,
    EditOrganizationComponent,
    AddCategoryComponent,
    EditCategoryComponent,
    ListCategoryComponent,
    ListSalesComponent,
    AddSalesComponent,
    EditSalesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgxChartsModule,
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
