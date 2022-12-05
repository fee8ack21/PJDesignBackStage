import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResponseBase } from '../../models/bases';
import { CategoryDialogData } from '../../models/category-dialog-data';
import { CreateCategoryRequest } from '../../models/create-category';
import { FormControlErrorType, StatusCode } from '../../models/enums';
import { UpdateCategoriesRequest } from '../../models/update-categories';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { ValidatorService } from '../../services/validator.service';

@Component({
  selector: 'app-category-dialog',
  templateUrl: './category-dialog.component.html',
  styleUrls: ['./category-dialog.component.scss']
})
export class CategoryDialogComponent implements OnInit {
  categoryForm: FormGroup;

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: CategoryDialogData,
    public dialogRef: MatDialogRef<CategoryDialogComponent>,
    public validatorService: ValidatorService,
    private httpService: HttpService,
    private snackBarService: SnackBarService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.categoryForm = new FormGroup(this.getInitFormControlData());
  }

  getInitFormControlData() {
    if (this.data.isEdit) {
      let categoryData: any = {};
      this.data.categories?.forEach(category => {
        categoryData[`category${category.id}isEnabled`] = new FormControl(category.isEnabled);
        categoryData[`category${category.id}name`] = new FormControl(category.name);
      })

      return categoryData;
    }

    return { name: new FormControl(null, [Validators.required]), isEnabled: new FormControl(true, [Validators.required]) }
  }

  onSubmit() {
    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    if (!this.data.isEdit) {
      let request = new CreateCategoryRequest();
      request.unitId = this.data.unitId;
      request.name = this.categoryForm.value['name'];
      request.isEnabled = this.categoryForm.value['isEnabled'];

      this.httpService.post<ResponseBase<string>>('category/createCategory', request).subscribe(response => {
        if (response.statusCode == StatusCode.Fail) {
          this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
          return;
        }
        this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      })
    } else {
      let requests: UpdateCategoriesRequest[] = [];

      this.data.categories?.forEach(category => {
        let temp = new UpdateCategoriesRequest();
        temp.id = category.id;
        temp.name = this.categoryForm.get(`category${category.id}name`)?.value;
        temp.isEnabled = this.categoryForm.get(`category${category.id}isEnabled`)?.value;
        requests.push(temp);
      })

      this.httpService.post<ResponseBase<string>>('category/UpdateCategories', requests).subscribe(response => {
        if (response.statusCode == StatusCode.Fail) {
          this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
          return;
        }
        this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      })
    }

    this.dialogRef.close(true);
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }
}
