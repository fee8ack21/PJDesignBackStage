import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControlErrorType } from 'src/app/shared/models/enums';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { UnitDialogData } from '../../models/unit-dialog-data';

@Component({
  selector: 'app-unit-dialog',
  templateUrl: './unit-dialog.component.html',
  styleUrls: ['./unit-dialog.component.scss']
})
export class UnitDialogComponent implements OnInit {
  unitForm: FormGroup;

  readonly templateOption = [{ name: '無', value: 0 }, { name: '單一編輯器', value: 1 }, { name: '列表 + 內頁', value: 2 }]

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  constructor(
    public dialogRef: MatDialogRef<UnitDialogComponent>,
    public validatorService: ValidatorService,
    @Inject(MAT_DIALOG_DATA) public data: UnitDialogData) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.unitForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      templateType: new FormControl(0, [Validators.required]),
      url: new FormControl(null),
      isAnotherWindow: new FormControl(false, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
    });
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }
}
