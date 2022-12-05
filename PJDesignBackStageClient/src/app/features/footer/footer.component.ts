import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ReviewNoteDialogComponent } from 'src/app/shared/components/review-note-dialog/review-note-dialog.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { CreateOrUpdateSetting } from 'src/app/shared/models/create-or-update-setting';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetSettingByUnitIdResponse } from 'src/app/shared/models/get-setting-by-unit-id';
import { ReviewNote } from 'src/app/shared/models/review-note';
import { ReviewNoteDialogData } from 'src/app/shared/models/review-note-dialog-data';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { GetType2UnitsResponse } from './feature-shared/models/get-type2-units';
import { FooterSocialIcon, FooterSettings } from './feature-shared/update-footer-settings';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent extends ListBaseComponent implements OnInit {
  administrator: { id: number, name: string } | null;
  settingEditorId?: number;
  settingEditorName?: string;
  settingCreateDt?: Date | null;
  settingReviewNote: string | null;
  settingReviewNotes: ReviewNote[] = [];
  unitId: number;

  footerForm: FormGroup;
  type2Units: { id: number, name: string, selected: boolean }[] = [];
  socialIcons: FooterSocialIcon[] = [];

  settingStatus?: EditStatus;

  @ViewChild('displayUnits') displayUnits: MatSelectionList;

  constructor(
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private snackBarService: SnackBarService,
    private unitService: UnitService,
    private authService: AuthService,
    private progressBarService: ProgressBarService,
    public dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.setAdministrator();
    this.listenUnitService();
    this.initForm();
  }

  setAdministrator() {
    this.administrator = this.authService.getAdministrator();
  }

  listenUnitService() {
    this.unitService.isBackStageUnitsInit.subscribe(async response => {
      this.setUnitId();
      await this.setType2Units();
      this.getSettingByUnitId();
    });
  }

  setUnitId() {
    this.unitId = this.unitService.getCurrentUnit();
  }

  async setType2Units() {
    const type2UnitsRespoonse = await this.getType2UnitsPromise();
    if (type2UnitsRespoonse.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
    } else {
      this.type2Units = [{ id: -1, name: '網站導覽', selected: false }];
      type2UnitsRespoonse.entries?.forEach(item => { this.type2Units.push({ ...item, selected: false }); });
    }
  }

  getType2UnitsPromise() {
    return this.httpService.get<ResponseBase<GetType2UnitsResponse[]>>('unit/getType2Units').toPromise();
  }

  getSettingByUnitId() {
    if (this.unitId == null || this.unitId == -1) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.settingEditorId = response.entries?.editorId;
      this.settingEditorName = response.entries?.editorName;
      this.settingCreateDt = response.entries?.createDt;
      this.settingStatus = response.entries?.status;
      this.settingReviewNotes = response.entries?.notes as ReviewNote[] ?? [];

      this.handleFormStatus()

      if (response.entries?.content != null) {
        this.updateFooterSetting(response.entries.content as FooterSettings);
      }
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

  handleFormStatus() {
    if (this.isInputDisabled()) {
      this.footerForm.disable();
      return;
    }
    this.footerForm.enable();
  }

  isInputDisabled(): boolean {
    return this.settingStatus == EditStatus.Review || (this.settingStatus == EditStatus.Reject && this.administrator?.id != this.settingEditorId);
  }

  updateFooterSetting(setting: FooterSettings) {
    this.footerForm.patchValue({ address: setting.address, phone: setting.phone, email: setting.email, fanpage: setting.fanpage });

    if (setting.isShowMapUnit) { this.type2Units[0].selected = true; }

    setting.showedUnits.forEach(showedUnit => {
      this.type2Units.forEach(unit => {
        if (showedUnit == unit.id) {
          unit.selected = true;
        }
      })
    })

    this.socialIcons = setting.socialIcons;
  }

  getSelectedDisplayUnits() {
    let units: number[] = [];
    if (this.displayUnits?.selectedOptions?.selected == null) { return units };

    this.displayUnits.selectedOptions.selected.forEach(item => {
      units.push(item.value)
    })
    return units;
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

  async updateFooterSettings(status = EditStatus.Review) {
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.snackBarService.showSnackBar('請填寫備註');
      return;
    }

    let settings = new FooterSettings();
    settings = { ... this.footerForm.value };

    let selectedUnits = this.getSelectedDisplayUnits();
    settings.isShowMapUnit = selectedUnits.includes(-1);
    settings.showedUnits = selectedUnits.filter(x => x != -1);
    settings.socialIcons = this.socialIcons.filter(x => x.url != null && x.url.trim().length > 0 && x.image != null && x.image.trim().length > 0);


    let request = new CreateOrUpdateSetting();
    request.unitId = this.unitId;
    request.content = settings;
    request.editStatus = status;

    if (status == EditStatus.Reject) {
      let temp = new ReviewNote();
      temp.Date = new Date();
      temp.Note = this.settingReviewNote!;
      temp.Name = this.administrator!.name
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

  isReviewNoteEmpty(): boolean {
    return this.settingReviewNote == null || this.settingReviewNote.trim().length == 0;
  }

  openReviewNoteDialog() {
    let data = new ReviewNoteDialogData();
    data.editorName = this.settingEditorName;
    data.notes = this.settingReviewNotes;
    data.createDt = this.settingCreateDt;

    this.dialog.open(ReviewNoteDialogComponent, {
      width: '474px',
      data: data
    });
  }
}
