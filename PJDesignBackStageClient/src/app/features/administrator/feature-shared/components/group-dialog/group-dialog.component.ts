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
  groupForm: FormGroup;

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
    this.groupForm = new FormGroup(this.getInitFormControlData(this.data));
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
    let request = new GetUnitRightsByGroupIdRequest();
    request.id = id;

    return this.httpService.post<ResponseBase<GetUnitRightsByGroupIdResponse[]>>('administrator/getUnitRightsByGroupId', request).toPromise();
  }

  async updateUnitRights(unitRights?: GetUnitRightsByGroupIdResponse[]) {
    let patchValues: any = {}

    this.data.units.forEach(unit => {
      const key = 'unit' + unit.id;
      patchValues[key] = unitRights?.find(unitRight => unitRight.unitId == unit.id)?.rightId ?? this.rightDefaultId;
    })

    this.groupForm.patchValue(patchValues);
  }

  onGroupSelectChange() {
    if (!this.data.isEdit) { return; }

    this.groupForm.get('id')?.valueChanges.subscribe(async value => {
      this.groupForm.patchValue({ name: this.data?.groups?.find(x => x.id == value)?.name })

      const unitRightsResponse = await this.getGroupUnitRightByIdPromise(value);

      if (unitRightsResponse?.statusCode == StatusCode.Fail) { return; }

      this.updateUnitRights(unitRightsResponse.entries);
    });
  }

  onSubmit() {
    if (this.groupForm.invalid) {
      this.groupForm.markAllAsTouched();
      return;
    }

    let request = new CreateOrUpdateGroupRequest();
    request.id = this.groupForm.get('id')?.value;
    request.name = this.groupForm.get('name')?.value;

    for (let key in this.groupForm.value) {
      if (!key.includes('unit')) { continue; }

      const unitId = parseInt(key.replace('unit', ""));
      const rightId = this.groupForm.value[key];

      if (rightId == this.rightDefaultId) { continue; }

      let temp = new UnitRight();
      temp.unitId = unitId;
      temp.rightId = rightId;

      request.unitRights.push(temp);
    }

    this.httpService.post<ResponseBase<string>>('administrator/createOrUpdateGroup', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      } else {
        this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      }
    })
    this.dialogRef.close(true);
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }
}
