import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode, TemplateType } from 'src/app/shared/models/enums';
import { GetSettingByUnitIdResponse } from 'src/app/shared/models/get-setting-by-unit-id';
import { GetUnitsRequest, GetUnitsResponse } from 'src/app/shared/models/get-units';
import { ReviewNote } from 'src/app/shared/models/review-note';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends DetailBaseComponent implements OnInit {
  type2Units: { id: number, name: string }[] = [];
  type2TemplateTypes = [{ id: 1, name: '左圖右文' }, { id: 2, name: '右圖左文' }, { id: 3, name: '三項目' }]

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
  }

  getType2UnitsPromise() {
    let request = new GetUnitsRequest(TemplateType.模板二);
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }
  handleUnitsResponse(response: ResponseBase<GetUnitsResponse[]>) {
    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }

    if (this.type2Units.length > 0) { return; }

    response.entries?.forEach(item => { this.type2Units.push({ ...item }); });
  }
}
