import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
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
import { ProgressBarService } from '../../services/progress-bar.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { DetailBaseComponent } from '../base/detail-base.component';

@Component({
  selector: 'app-unit-setting-form',
  templateUrl: './unit-setting-form.component.html',
  styleUrls: ['./unit-setting-form.component.scss']
})
export class UnitSettingFormComponent extends DetailBaseComponent implements OnInit {
  enName = '';
  imageFile: File;
  imageName: string;
  imagePath: string;

  @Input('unitId') unitId = -1;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    protected dialog: MatDialog) {
    super(route, authService, unitService, dialog);
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if ('unitId' in changes) {
      this.unitId = changes.unitId.currentValue;
      this.getSettingByUnitId();
    }
  }

  getSettingByUnitId() {
    if (this.unitId == null || this.unitId == -1) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.editorId = response.entries?.editorId;
      this.editorName = response.entries?.editorName;
      this.contentCreateDt = response.entries?.createDt;
      this.editStatus = response.entries?.editStatus;
      this.editReviewNotes = toCamel(response.entries?.notes) as ReviewNote[] ?? [];

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
    this.imageFile = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    this.imageName = this.imageFile.name;
    this.httpService.postPhoto<ResponseBase<string>>('upload/uploadPhoto', this.imageFile).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
      this.imagePath = response.entries!;
    })
  }

  onSubmit(status = EditStatus.Review) {
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.snackBarService.showSnackBar('請填寫備註');
      return;
    }

    let request = new CreateOrUpdateSettingRequest();
    let content = new UnitSetting();
    content.enName = this.enName;
    content.backgroundImageUrl = this.imagePath;
    request.content = content;
    request.unitId = this.unitId;
    request.editStatus = status;

    if (status == EditStatus.Reject) {
      let temp = new ReviewNote();
      temp.date = new Date();
      temp.note = this.editReviewNote!;
      temp.name = this.administrator!.name
      request.note = temp;
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
