import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddCategory } from '../../models/add-category.model';
import { Subscription } from 'rxjs';
import { CategoryService } from '../../services/category.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent {

  model: AddCategory;
  isImageSelectorVisible: boolean = false;

  imageSelectorSubscription$?: Subscription;
  
  constructor(private categoryService: CategoryService,
              private router: Router) {
       this.model = {
        categoryName: '',
       }      
  }

  onFormSubmit(): void {
    console.log(this.model);
    this.categoryService.createCategory(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/categories');
        }
      });
  }
}
