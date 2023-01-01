import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { CreateOrUpdateSettingRequest } from 'src/app/shared/models/create-or-update-setting';
import { EditStatus, StatusCode, TemplateType } from 'src/app/shared/models/enums';
import { GetSettingByUnitIdResponse } from 'src/app/shared/models/get-setting-by-unit-id';
import { GetUnitsRequest, GetUnitsResponse } from 'src/app/shared/models/get-units';
import { ReviewNote } from 'src/app/shared/models/review-note';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { FooterSocialIcon, FooterSettings } from './feature-shared/models/update-footer-settings';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent extends DetailBaseComponent implements OnInit {
  footerForm: FormGroup;
  type2Units: { id: number, name: string, selected: boolean }[] = [];
  socialIcons: FooterSocialIcon[] = [];

  @ViewChild('unitSelectEle') unitSelectEle: MatSelectionList;

  constructor(
    protected httpService: HttpService,
    public validatorService: ValidatorService,
    protected snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    protected dialog: MatDialog) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(async response => {
      this.setUnit();
      await this.setType2Units();
      this.getSettingByUnitId();
    });

    this.initForm();
  }

  async setType2Units() {
    const type2UnitsRespoonse = await this.getType2UnitsPromise();
    if (type2UnitsRespoonse.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }

    this.type2Units = [{ id: -1, name: '網站導覽', selected: false }];
    type2UnitsRespoonse.entries?.forEach(item => { this.type2Units.push({ ...item, selected: false }); });
  }

  getType2UnitsPromise() {
    let request = new GetUnitsRequest(undefined, TemplateType.模板二);
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }

  getSettingByUnitId() {
    if (!this.isUnitInit()) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.setEditData(
        response.entries?.editorId,
        response.entries?.editorName,
        response.entries?.createDt,
        response.entries?.editStatus,
        response.entries?.notes as ReviewNote[] ?? []
      );

      this.handleFormStatus(this.footerForm);

      if (response.entries?.content == null) { return; }

      this.updateFooterSetting(response.entries.content as FooterSettings);
    });
  }

  initForm() {
    this.footerForm = new FormGroup({
      address: new FormControl(null),
      phone: new FormControl(null),
      email: new FormControl(null),
      fanpage: new FormControl(null),
    });
  }

  updateFooterSetting(setting: FooterSettings) {
    this.footerForm.patchValue({ address: setting.address, phone: setting.phone, email: setting.email, fanpage: setting.fanpage });

    if (setting.isShowMapUnit) { this.type2Units[0].selected = true; }

    if (setting.showedUnits) {
      setting.showedUnits.forEach(showedUnit => {
        this.type2Units.forEach(unit => {
          if (showedUnit == unit.id) {
            unit.selected = true;
          }
        })
      })
    }

    this.socialIcons = setting.socialIcons;
  }

  addSocialIcon() {
    if (this.socialIcons.length >= 3) { this.snackBarService.showSnackBar('社群Icon最多三項'); return; }
    this.socialIcons.push(new FooterSocialIcon())
  }

  removeSocialIcon(index: number) {
    this.socialIcons.splice(index, 1);
  }

  onPhotoUpload(e: any, index: number) {
    const file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    const pattern = /image-*/;
    const reader = new FileReader();
    if (!file.type.match(pattern)) {
      this.snackBarService.showSnackBar('圖片格式錯誤');
      return;
    }
    reader.onload = (_e) => {
      this._handleReaderLoaded.bind(this);
      this._handleReaderLoaded(_e, e, index)
    };
    reader.readAsDataURL(file);
  }
  _handleReaderLoaded(e: any, inputEvent: any, index: number) {
    const reader = e.target;
    const image = new Image();
    const _this = this;

    image.src = reader.result;
    image.onload = function () {
      const height = image.height;
      const width = image.width;
      if (height > 300 || width > 300) {
        _this.snackBarService.showSnackBar('不符合圖片限制');
        inputEvent.target.value = '';
        return;
      }

      _this.socialIcons[index].image = reader.result;
    };
  }

  async onSubmit(status = EditStatus.Review) {
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.editReviewNoteErrFlag = true;
      return;
    }

    let settings: FooterSettings = { ... this.footerForm.value };
    let selectedUnits = this.getListSelectedIDs(this.unitSelectEle);
    settings.isShowMapUnit = selectedUnits.includes(-1);
    settings.showedUnits = selectedUnits.filter(x => x != -1);
    settings.socialIcons = this.socialIcons.filter(x => x.url != null && x.url.trim().length > 0 && x.image != null && x.image.trim().length > 0);

    let request = new CreateOrUpdateSettingRequest(this.unit.id, settings, status);

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
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
