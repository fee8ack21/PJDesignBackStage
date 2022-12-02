import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatSelectionList } from '@angular/material/list';
import { Router } from '@angular/router';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { CreateOrUpdateSetting } from 'src/app/shared/models/create-or-update-setting';
import { StatusCode, UnitID } from 'src/app/shared/models/enums';
import { GetSettingByUnitIdResponse } from 'src/app/shared/models/get-setting-by-unit-id';
import { HttpService } from 'src/app/shared/services/http.service';
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
  unitId: number;
  footerForm: FormGroup;
  type2Units: { id: number, name: string, selected: boolean }[] = [];
  socialIcons: FooterSocialIcon[] = [];

  @ViewChild('displayUnits') displayUnits: MatSelectionList;

  constructor(
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private snackBarService: SnackBarService,
    private unitService: UnitService,
    private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
    this.setUnitId();
    this.getType2Units();
  }

  setUnitId() {
    this.unitId = this.unitService.getCurrentUnit();
  }

  getType2Units() {
    this.httpService.get<ResponseBase<GetType2UnitsResponse[]>>('unit/getType2Units').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.type2Units = [{ id: -1, name: '網站導覽', selected: false }];
      response.entries?.forEach(item => { this.type2Units.push({ ...item, selected: false }); });
      this.getFooterSetting();

    });
  }

  getFooterSetting() {
    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${UnitID.Footer設定}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      if (response.entries?.content != null) {
        this.updateFooterSetting(response.entries.content as FooterSettings);
      }
    });
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

  initForm() {
    this.footerForm = new FormGroup({
      address: new FormControl(null),
      phone: new FormControl(null),
      email: new FormControl(null),
      fanpage: new FormControl(null),
    });
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

  validateFooterSettings() {
    let isIconFormatCorrect = true;
    this.socialIcons.forEach(item => {
      if (item.url == null || item.url.trim().length == 0 || item.image == null || item.image.trim().length == 0) {
        isIconFormatCorrect = false;
        return;
      }
    })

    if (!isIconFormatCorrect) {
      this.snackBarService.showSnackBar('請正確填寫圖片欄位');
      return;
    }
  }

  async updateFooterSettings() {
    let settings = new FooterSettings();
    settings = { ... this.footerForm.value };

    let selectedUnits = this.getSelectedDisplayUnits();
    settings.isShowMapUnit = selectedUnits.includes(-1);
    settings.showedUnits = selectedUnits.filter(x => x != -1);
    settings.socialIcons = this.socialIcons.filter(x => x.url != null && x.url.trim().length > 0 && x.image != null && x.image.trim().length > 0);


    let request = new CreateOrUpdateSetting();
    request.unitId = UnitID.Footer設定;
    request.content = settings;
    this.httpService.post<ResponseBase<string>>('unit/createOrUpdateSetting', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText)
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
    })
  }
}
