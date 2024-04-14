import { Component, OnDestroy, OnInit } from '@angular/core';
import { Category } from '../../models/category.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { EditCategory } from '../../models/edit-category.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {
  id: number | null = null;
  model?: Category;

  routerSubscription?: Subscription;
  editCategorySubscription?: Subscription;
  getCategorySubscription?: Subscription;
  deleteCategorySubscription?: Subscription;

  constructor(private route: ActivatedRoute,
              private categoryService: CategoryService,
              private router: Router) {

  }

  onFormSubmit(): void {
    // Convert this model to request object
    if (this.model && this.id) {
      var editCategory: EditCategory = {
        categoryName: this.model.categoryName
      };

      this.editCategorySubscription = this.categoryService.updateCategory(this.id, editCategory)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/categories');
          }
        });
    }
  }

  ngOnInit(): void {
    this.routerSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        const paramId = params.get('id');
        this.id = paramId !== null ? parseInt(paramId, 10) : null;

        // Get Organization API
        if (this.id) {
          this.getCategorySubscription = this.categoryService.getCategoryById(this.id)
            .subscribe({
              next: (response) => {
                this.model = response;
              }
            });
        }
      }
    })    
  }

  onDelete(): void {
    if (this.id) {
      // call service and delete blogpost
      this.deleteCategorySubscription = this.categoryService.deleteCategory(this.id)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/categories');
          }
        });
    }
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
    this.editCategorySubscription?.unsubscribe();
    this.deleteCategorySubscription?.unsubscribe();
    this.getCategorySubscription?.unsubscribe();
  }
}
