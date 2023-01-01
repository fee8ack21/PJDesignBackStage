import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/shared/components/base/base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode, TemplateType } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateUnitRequest } from '../../models/create-or-update-unit';
import { UnitDialogData } from '../../models/unit-dialog-data';

@Component({
  selector: 'app-unit-dialog',
  templateUrl: './unit-dialog.component.html',
  styleUrls: ['./unit-dialog.component.scss']
})
export class UnitDialogComponent extends BaseComponent implements OnInit {
  form: FormGroup;
  templateOption = [{ name: '無', value: TemplateType.無 }, { name: '單一編輯器', value: TemplateType.模板一 }, { name: '列表 + 內頁', value: TemplateType.模板二 }]

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    protected unitService: UnitService,
    public dialogRef: MatDialogRef<UnitDialogComponent>,
    public validatorService: ValidatorService,
    @Inject(MAT_DIALOG_DATA) public data: UnitDialogData) {
    super(unitService);
  }

  ngOnInit(): void {
    this.updateTemplateOption();
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      id: new FormControl(this.data.unit?.id),
      parent: new FormControl(this.data.parent),
      name: new FormControl(this.data.unit?.name ?? null, [Validators.required]),
      templateType: new FormControl(
        {
          value: this.data.unit?.templateType ?? 0,
          disabled: this.data.unit?.templateType == TemplateType.固定單元
        }, [Validators.required],),
      url: new FormControl(this.data.unit?.templateType == TemplateType.無 ? this.data.unit?.frontStageUrl ?? null : null),
      isAnotherWindow: new FormControl(this.data.unit?.isAnotherWindow ?? false, [Validators.required]),
      isEnabled: new FormControl(this.data.unit?.isEnabled ?? true, [Validators.required]),
    });
  }

  updateTemplateOption() {
    if (this.data.unit?.templateType == TemplateType.固定單元) {
      this.templateOption.push({ name: '固定單元', value: TemplateType.固定單元 });
    }
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    let request: CreateOrUpdateUnitRequest = { ...this.form.value };

    this.httpService.post<ResponseBase<string>>('unit/createOrUpdateUnit', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar((response.message && response.message.length > 0) ? response.message : SnackBarService.RequestFailedText);
        return;
      }

      this.unitService.getBackStageUnitsByGroupId(true);
      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.dialogRef.close(true);
    })
  }
}
