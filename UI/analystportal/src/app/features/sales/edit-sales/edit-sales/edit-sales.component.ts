import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditSales } from '../../models/edit-sales.model';
import { Organization } from 'src/app/features/organization/models/organization.model';
import { Observable, Subscription } from 'rxjs';
import { Category } from 'src/app/features/category/models/category.model';
import { SalesService } from '../../services/sales.service';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { OrganizationService } from 'src/app/features/organization/services/organization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Sales } from '../../models/sales.model';

@Component({
  selector: 'app-edit-sales',
  templateUrl: './edit-sales.component.html',
  styleUrls: ['./edit-sales.component.css']
})
export class EditSalesComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: Sales;
  organizations$?: Observable<Organization[]>;
  categories$?: Observable<Category[]>;
  selectedCategoryId: number = -1;
  selectedOrganizations?: number[];

  routeSubscription?: Subscription;
  updateSalesSubscription?: Subscription;
  getSalesSubscription?: Subscription;
  deleteSalesSubscription?: Subscription;

  constructor(private salesService: SalesService,
              private categoryService: CategoryService,
              private organizationService: OrganizationService,
              private router: Router,
              private route: ActivatedRoute,) {
  }

  ngOnInit(): void {
    this.organizations$ = this.organizationService.getAllOrganizations();
    this.categories$ = this.categoryService.getAllCategories();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        // Get Sales From API
        if (this.id) {
          this.getSalesSubscription = this.salesService.getSalesById(this.id).subscribe({
            next: (response) => {
              this.model = response;
              this.selectedCategoryId = response.categoryId;
              this.selectedOrganizations = response.organizations.map(x => x.id);
              console.log(this.selectedOrganizations);
            }
          });
        }
      }
    });   
  }

  onFormSubmit(): void {
    // Convert this model to Request Object
    if (this.model && this.id) {
      this.model.total = Number(this.model.total.toFixed(2));
      // this.model.categoryId = this.selectedCategoryId;
      var updateSales: EditSales = {
        orderDate: this.model.orderDate,
        categoryId: this.model.categoryId,
        productName: this.model.productName,
        total: this.model.total,
        city: this.model.city,
        state: this.model.state,
        region: this.model.region,
        organizations: this.selectedOrganizations ?? []
      };

      this.updateSalesSubscription = this.salesService.updateSales(this.id, updateSales)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/sales');
        }
      });
    }
  }

  onDelete(): void {
    if (this.id) {
      // call service and delete blogpost
      this.deleteSalesSubscription = this.salesService.deleteSales(this.id)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/sales');
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateSalesSubscription?.unsubscribe();
    this.getSalesSubscription?.unsubscribe();
    this.deleteSalesSubscription?.unsubscribe();
  }
}
