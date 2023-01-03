import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { HomeSettingData, HomeSettings, homeType1and2DataProperties, homeType3and4DataProperties, homeType5DataProperties, homeType6DataProperties, homeType7DataProperties, homeType8and9and10DataProperties } from './feature-shared/models/update-home-settings';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends DetailBaseComponent implements OnInit {
  sectionTypes = [
    { type: -1, name: '作品集' },
    { type: 1, name: '左文+右圖+背景' },
    { type: 2, name: '右文+左圖+背景' },
    { type: 3, name: '右文+左按鈕連結+背景' },
    { type: 4, name: '左文+右按鈕連結+背景' },
    { type: 5, name: '置中標題+敘述' },
    { type: 6, name: '小標題+小內文四區塊' },
    { type: 7, name: '中英標題+小圖案+小標題+小內文四區塊' },
    { type: 8, name: '左圖+右文(Type2單元)' },
    { type: 9, name: '右圖+左文(Type2單元)' },
    { type: 10, name: '三圖文(Type2單元)' },
  ]
  type2Units: { id: number, name: string }[] = [];
  sections: HomeSettings[] = [];

  constructor(
    protected httpService: HttpService,
    public validatorService: ValidatorService,
    protected snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    private spinnerService: SpinnerService,
    protected dialog: MatDialog) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(async response => {
      this.setUnit();
      this.fetchPageData();
    });
  }

  async fetchPageData() {
    this.spinnerService.isShow$.next(true);
    this.handleUnitsResponse(await this.getType2UnitsPromise());
    this.handleSettingResponse(await this.getSettingByUnitIdPromise());
    this.spinnerService.isShow$.next(false);
  }

  getSettingByUnitIdPromise() {
    if (!this.isUnitInit()) { return }
    return this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unit.id}`).toPromise();
  }
  handleSettingResponse(response: ResponseBase<GetSettingByUnitIdResponse> | undefined) {
    if (response == undefined) { return; }

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

    this.sections = response.entries?.content! as HomeSettings[];
  }

  getType2UnitsPromise() {
    let request = new GetUnitsRequest(undefined, TemplateType.模板二);
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }
  handleUnitsResponse(response: ResponseBase<GetUnitsResponse[]>) {
    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }

    if (this.type2Units.length > 0) { return; }

    response.entries?.forEach(item => { this.type2Units.push({ ...item }); });
    this.updateSectionTypes();
  }

  updateSectionTypes() {
    if (this.type2Units.length > 0) { return; }

    const types = [8, 9, 10];

    types.forEach(type => {
      const index = this.sectionTypes.findIndex(x => x.type == type);
      if (index == -1) { return; }
      this.sectionTypes.splice(index, 1);
    })
  }

  addSection() {
    if (!Array.isArray(this.sections)) {
      this.sections = [];
    }
    this.sections.push(new HomeSettings(this.sections.length + 1));
  }

  removeSection(id: number) {
    const index = this.sections.findIndex(x => x.id == id);
    if (index == -1) { return; }
    this.sections.splice(index, 1);
  }

  async onSubmit(status = EditStatus.Review) {
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.editReviewNoteErrFlag = true;
      return;
    }

    let request = new CreateOrUpdateSettingRequest(this.unit.id, this.getFilterdSettings(), status);

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
    }

    this.httpService.post<ResponseBase<string>>('unit/createOrUpdateSetting', request).subscribe(async response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(response.message ?? SnackBarService.RequestFailedText)
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.handleSettingResponse(await this.getSettingByUnitIdPromise());
    })
  }

  getFilterdSettings() {
    let settings: HomeSettings[] = []

    this.sections.forEach(section => {
      let temp: HomeSettings = { id: section.id, type: section.type, isEnabled: section.isEnabled, data: {} };
      let filterProperties: string[] = [];

      switch (section.type) {
        case 1:
        case 2:
          filterProperties = [...homeType1and2DataProperties];
          break;
        case 3:
        case 4:
          filterProperties = [...homeType3and4DataProperties];
          break;
        case 5:
          filterProperties = [...homeType5DataProperties];
          break;
        case 6:
          filterProperties = [...homeType6DataProperties];
          break;
        case 7:
          filterProperties = [...homeType7DataProperties];
          break;
        case 8:
        case 9:
        case 10:
          filterProperties = [...homeType8and9and10DataProperties];
          break;
      }

      for (let key in section.data) {
        if (filterProperties.includes(key)) {
          (temp.data[key as keyof HomeSettingData] as any) = section.data[key as keyof HomeSettingData];
        }
      }

      settings.push(temp);
    })

    return settings;
  }

  getSectionTypeName(type: number): string {
    let name = '';
    this.sectionTypes.forEach(item => {
      if (item.type == type) {
        name = item.name;
      }
    })
    return name;
  }

  onPhotoUpload(e: any, index: number, type: string) {
    const imageFile = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    const formData = new FormData();
    formData.append('image', imageFile, imageFile.name)

    this.httpService.post<ResponseBase<string>>('upload/uploadPhoto', formData, { headers: new HttpHeaders() }).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.setImageData(index, type, imageFile.name, response.entries!);
    })
  }

  setImageData(id: number, type: string, name: string, path: string) {
    const section = this.sections.find(x => x.id == id);

    if (section == undefined) { return; }

    switch (type) {
      case 'image':
        section.data.imageName = name;
        section.data.imageUrl = path;
        break;
      case 'backgroundImage':
        section.data.backgroundImageName = name;
        section.data.backgroundImageUrl = path;
        break;
      case 'smallIcon1':
        section.data.smallIconName1 = name;
        section.data.smallIconUrl1 = path;
        break;
      case 'smallIcon2':
        section.data.smallIconName2 = name;
        section.data.smallIconUrl2 = path;
        break;
      case 'smallIcon3':
        section.data.smallIconName3 = name;
        section.data.smallIconUrl3 = path;
        break;
      case 'smallIcon4':
        section.data.smallIconName4 = name;
        section.data.smallIconUrl4 = path;
        break;
    }
  }
}
