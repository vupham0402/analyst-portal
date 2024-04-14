import { Component, OnInit } from '@angular/core';
import { Category } from '../../models/category.model';
import { Observable } from 'rxjs';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-list-category',
  templateUrl: './list-category.component.html',
  styleUrls: ['./list-category.component.css']
})
export class ListCategoryComponent implements OnInit {

  categories$?: Observable<Category[]>;

  constructor(private categoryService: CategoryService) {

  }

  ngOnInit(): void {
    // get all organizations from API
    this.categories$ = this.categoryService.getAllCategories();
  }
}
