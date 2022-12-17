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
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    protected dialog: MatDialog) {
    super(route, authService, unitService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit.subscribe(async response => {
      this.setUnit();
      this.getSettingByUnitId();
      await this.setType2Units();
    });
  }

  getSettingByUnitId() {
    if (this.unit.id == null || this.unit.id == -1) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.editorId = response.entries?.editorId;
      this.editorName = response.entries?.editorName;
      this.contentCreateDt = response.entries?.createDt;
      this.editStatus = response.entries?.editStatus;
      this.editReviewNotes = response.entries?.notes as ReviewNote[] ?? [];
    });
  }

  async setType2Units() {
    const type2UnitsRespoonse = await this.getType2UnitsPromise();
    if (type2UnitsRespoonse.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
    } else {
      if (this.type2Units.length > 0) { return; }
      type2UnitsRespoonse.entries?.forEach(item => { this.type2Units.push({ ...item }); });
    }
  }

  getType2UnitsPromise() {
    let request = new GetUnitsRequest();
    request.templateType = TemplateType.模板二;
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }
}
