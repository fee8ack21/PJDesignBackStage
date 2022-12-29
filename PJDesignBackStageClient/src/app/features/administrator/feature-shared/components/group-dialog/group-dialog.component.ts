import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResponseBase } from 'src/app/shared/models/bases';
import { FormControlErrorType, StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateGroupRequest, UnitRight } from '../../models/create-or-update-group';
import { GetUnitRightsByGroupIdRequest, GetUnitRightsByGroupIdResponse } from '../../models/get-unit-rights-by-group-id';
import { GroupDialogData } from '../../models/group-dialog-data';

@Component({
  selector: 'app-group-dialog',
  templateUrl: './group-dialog.component.html',
  styleUrls: ['./group-dialog.component.scss']
})
export class GroupDialogComponent implements OnInit {
  readonly rightDefaultId = -1;
  form: FormGroup;

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  constructor(
    public dialogRef: MatDialogRef<GroupDialogComponent>,
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    public validatorService: ValidatorService,
    @Inject(MAT_DIALOG_DATA) public data: GroupDialogData) { }

  ngOnInit(): void {
    this.initForm();
    this.initFirstGroupData();
    this.onGroupSelectChange();
  }

  initForm() {
    this.form = new FormGroup(this.getInitFormControlData(this.data));
  }

  getInitFormControlData(data: GroupDialogData) {
    let groupData: any = {};
    groupData.id = new FormControl(data.id);
    groupData.name = new FormControl(data.name, [Validators.required]);

    data.units.forEach(unit => {
      groupData['unit' + unit.id] = new FormControl(this.rightDefaultId);
    })

    return groupData;
  }

  async initFirstGroupData() {
    if (!this.data.isEdit) { return; }
    if (this.data?.groups == null || this.data.groups.length == 0) { return; }

    const unitRightsResponse = await this.getGroupUnitRightByIdPromise(this.data.groups[0].id);

    if (unitRightsResponse?.statusCode == StatusCode.Fail) { return; }
    if (unitRightsResponse.entries!.length == 0) { return; }

    this.updateUnitRights(unitRightsResponse.entries);
  }

  getGroupUnitRightByIdPromise(id: number) {
    let request = new GetUnitRightsByGroupIdRequest(id);
    return this.httpService.post<ResponseBase<GetUnitRightsByGroupIdResponse[]>>('administrator/getUnitRightsByGroupId', request).toPromise();
  }

  async updateUnitRights(unitRights?: GetUnitRightsByGroupIdResponse[]) {
    let patchValues: any = {}

    this.data.units.forEach(unit => {
      const key = 'unit' + unit.id;
      patchValues[key] = unitRights?.find(unitRight => unitRight.unitId == unit.id)?.rightId ?? this.rightDefaultId;
    })

    this.form.patchValue(patchValues);
  }

  onGroupSelectChange() {
    if (!this.data.isEdit) { return; }

    this.form.get('id')?.valueChanges.subscribe(async value => {
      this.form.patchValue({ name: this.data?.groups?.find(x => x.id == value)?.name })

      const unitRightsResponse = await this.getGroupUnitRightByIdPromise(value);

      if (unitRightsResponse?.statusCode == StatusCode.Fail) { return; }

      this.updateUnitRights(unitRightsResponse.entries);
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    let request = new CreateOrUpdateGroupRequest(this.form.get('id')?.value, this.form.get('name')?.value);

    for (let key in this.form.value) {
      if (!key.includes('unit')) { continue; }

      const unitId = parseInt(key.replace('unit', ""));
      const rightId = this.form.value[key];

      if (rightId == this.rightDefaultId) { continue; }

      request.unitRights.push(new UnitRight(unitId, rightId));
    }

    this.httpService.post<ResponseBase<string>>('administrator/createOrUpdateGroup', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
    })
    this.dialogRef.close(true);
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }
}
