import { Component, OnDestroy, OnInit } from '@angular/core';
import { Sales } from '../../models/sales.model';
import { Observable } from 'rxjs';
import { Organization } from 'src/app/features/organization/models/organization.model';
import { SalesService } from '../../services/sales.service';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { OrganizationService } from 'src/app/features/organization/services/organization.service';
import { Router } from '@angular/router';
import { AddSales } from '../../models/add-sales.model';
import { Category } from 'src/app/features/category/models/category.model';

@Component({
  selector: 'app-add-sales',
  templateUrl: './add-sales.component.html',
  styleUrls: ['./add-sales.component.css']
})
export class AddSalesComponent implements OnInit {
  model: AddSales;
  organizations$?: Observable<Organization[]>;
  categories$?: Observable<Category[]>;
  selectedCategoryId: number = -1;

  constructor(private salesService: SalesService,
              private categoryService: CategoryService,
              private organizationService: OrganizationService,
              private router: Router) {
    this.model = {
      id: '',
      orderDate: new Date(),
      categoryId: this.selectedCategoryId,
      productName: '',
      total: 0,
      city: '',
      state: '',
      region: '',
      organizations: []
    }
  }

  ngOnInit(): void {
    this.organizations$ = this.organizationService.getAllOrganizations();
    this.categories$ = this.categoryService.getAllCategories();
  }

  onFormSubmit(): void {
    this.model.total = Number(this.model.total.toFixed(2));
    this.model.categoryId = this.selectedCategoryId;
    // console.log(this.model);
    this.salesService.createSales(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/sales');
        }
      })
  }
}
