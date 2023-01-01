import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResponseBase } from '../../models/bases';
import { CategoryDialogData } from '../../models/category-dialog-data';
import { CreateCategoryRequest } from '../../models/create-category';
import { StatusCode } from '../../models/enums';
import { UpdateCategoriesRequest } from '../../models/update-categories';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { ValidatorService } from '../../services/validator.service';
import { BaseComponent } from '../base/base.component';

@Component({
  selector: 'app-category-dialog',
  templateUrl: './category-dialog.component.html',
  styleUrls: ['./category-dialog.component.scss']
})
export class CategoryDialogComponent extends BaseComponent implements OnInit {
  form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: CategoryDialogData,
    public dialogRef: MatDialogRef<CategoryDialogComponent>,
    public validatorService: ValidatorService,
    protected unitService: UnitService,
    private httpService: HttpService,
    private snackBarService: SnackBarService) {
    super(unitService);
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup(this.getInitFormControlData());
  }

  getInitFormControlData() {
    if (!this.data.isEdit) {
      return { name: new FormControl(null, [Validators.required]), isEnabled: new FormControl(true, [Validators.required]) }
    }

    let categoryData: any = {};
    this.data.categories?.forEach(category => {
      categoryData[`category${category.id}isEnabled`] = new FormControl(category.isEnabled);
      categoryData[`category${category.id}name`] = new FormControl(category.name);
    })

    return categoryData;
  }

  onSubmit(): void {
    if (!this.validateForm(this.form)) { return; }

    if (!this.data.isEdit) {
      let request = new CreateCategoryRequest(this.data.unitId, this.form.value['name'], this.form.value['isEnabled']);

      this.httpService.post<ResponseBase<string>>('category/createCategory', request).subscribe(response => {
        if (response.statusCode == StatusCode.Fail) {
          this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
          return;
        }
        this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      });
    } else {
      let requests: UpdateCategoriesRequest[] = [];

      this.data.categories?.forEach(category => {
        requests.push(
          new UpdateCategoriesRequest(
            category.id,
            this.form.get(`category${category.id}name`)?.value,
            this.form.get(`category${category.id}isEnabled`)?.value));
      })

      this.httpService.post<ResponseBase<string>>('category/updateCategories', requests).subscribe(response => {
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
