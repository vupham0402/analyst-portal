import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Sales } from 'src/app/features/sales/models/sales.model';
import { SalesService } from 'src/app/features/sales/services/sales.service';
import { Organization } from '../../organization/models/organization.model';
import { OrganizationService } from '../../organization/services/organization.service';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';

@Component({
  selector: 'app-list-sales',
  templateUrl: './list-sales.component.html',
  styleUrls: ['./list-sales.component.css']
})
export class ListSalesComponent implements OnInit{
  sales$?: Observable<Sales[]>;
  organizations$?: Observable<Organization[]>;
  categories$?: Observable<Category[]>;

  selectedOrganizationId: number | undefined;
  selectedCategoryId: number | undefined;
  inputProductName: string | undefined;
  sortBy: string | undefined;
  sortDirection: string | undefined;

  totalCount?: number;
  pageNumber: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;
  inputPage: number = 1;


  constructor(private salesService: SalesService,
              private organizationService: OrganizationService,
              private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    this.loadSales();
    // get all organizations
    this.organizations$ = this.organizationService.getAllOrganizations();
    // get all categories
    this.categories$ = this.categoryService.getAllCategories();
  }

  loadSales(): void {
    this.salesService.getAllSales(this.selectedOrganizationId, this.selectedCategoryId, this.inputProductName, this.sortBy, this.sortDirection, this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          this.totalCount = response.length > 0 ? response[0].count : 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.sales$ = of(response);
        }
      })
  }

  onOrganizationSelect(event: Event): void {
    this.selectedOrganizationId = (event.target as HTMLSelectElement).value ? parseInt((event.target as HTMLSelectElement).value, 10) : undefined;
    this.loadSales();
  }

  onCategorySelect(event: Event): void {
    this.selectedCategoryId = (event.target as HTMLSelectElement).value ? parseInt((event.target as HTMLSelectElement).value, 10) : undefined;
    this.loadSales();
  }

  onProductNameInput(event: Event): void {
    this.inputProductName = (event.target as HTMLSelectElement).value;
  }

  sort(sortBy: string, sortDirection: string) {
    this.sales$ = this.salesService.getAllSales(this.selectedOrganizationId, this.selectedCategoryId, this.inputProductName, sortBy, sortDirection, this.pageNumber, this.pageSize);
  } 
  
  clear(): void {
    this.inputProductName = undefined;
    const inputElement = document.getElementById('productNameInput') as HTMLInputElement;
    if (inputElement) {
        inputElement.value = '';
    }
    this.loadSales();
  }

  reset(): void {
    this.selectedOrganizationId = undefined;
    this.selectedCategoryId = undefined;
    this.inputProductName = undefined;
    const orgSelection = document.getElementById('orgSelection') as HTMLInputElement;
    if (orgSelection) {
        orgSelection.value = '';
    }
    const catSelection = document.getElementById('catSelection') as HTMLInputElement;
    if (catSelection) {
        catSelection.value = '';
    }
    const inputElement = document.getElementById('productNameInput') as HTMLInputElement;
    if (inputElement) {
        inputElement.value = '';
    }
    this.loadSales();
  }

  goToPage(page: number): void {
      if (page >= 1 && page <= this.totalPages) {
          this.pageNumber = page;
          this.loadSales();
      }
  }

  previousPage(): void {
      if (this.pageNumber > 1) {
          this.goToPage(this.pageNumber - 1);
      }
  }

  nextPage(): void {
      if (this.pageNumber < this.totalPages) {
          this.goToPage(this.pageNumber + 1);
      }
  }

  getPageRange(): number[] {
      const range = 3; // Number of pages to show before and after the current page
      const start = Math.max(1, this.pageNumber - range);
      const end = Math.min(this.totalPages, this.pageNumber + range);

      const pageRange: number[] = [];
      for (let i = start; i <= end; i++) {
          pageRange.push(i);
      }

      return pageRange;
  }

  goToInputPage(): void {
    if (this.inputPage >= 1 && this.inputPage <= this.totalPages) {
        this.goToPage(this.inputPage);
    }
}
}
