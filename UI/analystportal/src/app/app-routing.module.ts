import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './features/public/dashboard/dashboard.component';
import { LoginComponent } from './features/auth/login/login.component';
import { authGuard } from './features/auth/guards/auth.guard';
import { ListOrganizationComponent } from './features/organization/list-organization/list-organization/list-organization.component';
import { AddOrganizationComponent } from './features/organization/add-organization/add-organization/add-organization.component';
import { EditOrganizationComponent } from './features/organization/edit-organization/edit-organization/edit-organization.component';
import { ListCategoryComponent } from './features/category/list-category/list-category/list-category.component';
import { AddCategoryComponent } from './features/category/add-category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category/edit-category.component';
import { ListSalesComponent } from './features/sales/list-sales/list-sales.component';
import { AddSalesComponent } from './features/sales/add-sales/add-sales/add-sales.component';
import { EditSalesComponent } from './features/sales/edit-sales/edit-sales/edit-sales.component';
import { RegisterComponent } from './features/auth/register/register.component';

const routes: Routes = [
  {
    path: "",
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "register",
    component: RegisterComponent
  },
  {
    path: "admin/organizations",
    component: ListOrganizationComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/organizations/add",
    component: AddOrganizationComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/organizations/:id",
    component: EditOrganizationComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/categories",
    component: ListCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/categories/add",
    component: AddCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/categories/:id",
    component: EditCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/sales",
    component: ListSalesComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/sales/add",
    component: AddSalesComponent,
    canActivate: [authGuard]
  },
  {
    path: "admin/sales/:id",
    component: EditSalesComponent,
    canActivate: [authGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
