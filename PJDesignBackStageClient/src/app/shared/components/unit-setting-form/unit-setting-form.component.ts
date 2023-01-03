import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { toCamel } from '../../helpers/format-helper';
import { ResponseBase } from '../../models/bases';
import { CreateOrUpdateSettingRequest } from '../../models/create-or-update-setting';
import { EditStatus, StatusCode } from '../../models/enums';
import { GetSettingByUnitIdResponse } from '../../models/get-setting-by-unit-id';
import { ReviewNote } from '../../models/review-note';
import { UnitSetting } from '../../models/unit-setting';
import { AuthService } from '../../services/auth.service';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { ValidatorService } from '../../services/validator.service';
import { DetailBaseComponent } from '../base/detail-base.component';

@Component({
  selector: 'app-unit-setting-form',
  templateUrl: './unit-setting-form.component.html',
  styleUrls: ['./unit-setting-form.component.scss']
})
export class UnitSettingFormComponent extends DetailBaseComponent implements OnInit, OnChanges {
  enName = '';
  imageFile: File;
  imageName: string;
  imagePath: string;

  @Input('unitId') unitId = -1;

  constructor(
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    protected dialog: MatDialog) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!('unitId' in changes)) { return; }
    this.unitId = changes.unitId.currentValue;
    this.getSettingByUnitId();
  }

  isUnitInit() {
    return this.unitId != null && this.unitId != -1;
  }

  getSettingByUnitId() {
    if (!this.isUnitInit()) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.setEditData(
        response.entries?.editorId,
        response.entries?.editorName,
        response.entries?.createDt,
        response.entries?.editStatus,
        response.entries?.notes as ReviewNote[] ?? [])
      this.updateSetting(response.entries?.content as UnitSetting)
    });
  }

  updateSetting(setting?: UnitSetting) {
    if (setting == null) { return; }

    this.enName = setting.enName;
    this.imagePath = setting.backgroundImageUrl;
    this.imageName = setting.backgroundImageUrl.split('/').pop() ?? '';
  }

  onPhotoUpload(e: any) {
    const file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];

    if (file == undefined) { return; }

    this.imageFile = file;
    this.imageName = this.imageFile.name;

    const formData = new FormData();
    formData.append('image', this.imageFile, this.imageFile.name);

    this.httpService.post<ResponseBase<string>>('upload/uploadPhoto', formData, { headers: new HttpHeaders() }).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
      this.imagePath = response.entries!;
    })
  }

  onSubmit(status = EditStatus.Review) {
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.editReviewNoteErrFlag = true;
      return;
    }

    let content = new UnitSetting(this.enName, this.imagePath);
    let request = new CreateOrUpdateSettingRequest(this.unitId, content, status);

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!, new Date());
    }

    this.httpService.post<ResponseBase<string>>('unit/createOrUpdateSetting', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(response.message ?? SnackBarService.RequestFailedText)
        return;
      }

      this.getSettingByUnitId();
      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
    })
  }
}
