import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ResponseBase } from '../models/bases';
import { StatusCode, TemplateType, UnitID } from '../models/enums';
import { GetBackStageUnitsByGroupIdResponse } from '../models/get-back-stage-units-by-group-id';
import { HttpService } from './http.service';
@Injectable()

export class UnitService {
  isBackStageUnitsInit = new BehaviorSubject<boolean>(false);
  private _units: GetBackStageUnitsByGroupIdResponse[];
  private _fixedUnits: GetBackStageUnitsByGroupIdResponse[] = [];
  private _customUnits: GetBackStageUnitsByGroupIdResponse[] = [];

  constructor(private httpService: HttpService) { }

  async getBackStageUnitsByGroupId(): Promise<{ fixedUnits: GetBackStageUnitsByGroupIdResponse[], customUnits: GetBackStageUnitsByGroupIdResponse[] }> {
    if (this._units != undefined) { return { fixedUnits: this._fixedUnits, customUnits: this._customUnits } };

    const response = await this.httpService.get<ResponseBase<GetBackStageUnitsByGroupIdResponse[]>>('unit/getBackStageUnitsByGroupId').toPromise();
    if (response.statusCode == StatusCode.Success) {
      this._units = response.entries!;
      this._setFormattedUnits(response.entries!)

      this.isBackStageUnitsInit.next(true);
      return { fixedUnits: this._fixedUnits, customUnits: this._customUnits };
    }

    return { fixedUnits: [], customUnits: [] };
  }

  getCurrentUnit() {
    if (this._units == null || this._units.length == 0) { return -1; }

    const path = window.location.pathname;
    const filtededUnits = this._units.filter(x => x.url == path);

    if (filtededUnits.length == 0) { return -1; }

    return filtededUnits[0].id;
  }

  private _setFormattedUnits(units: GetBackStageUnitsByGroupIdResponse[]) {
    let fixedUnits: GetBackStageUnitsByGroupIdResponse[] = []
    let customUnits: GetBackStageUnitsByGroupIdResponse[] = []
    let childUnits: GetBackStageUnitsByGroupIdResponse[] = []

    units.forEach(unit => {
      if (unit.parent) {
        childUnits.push(unit);
        return;
      }

      if (unit.templateType == TemplateType.固定單元) {
        fixedUnits.push(unit)
        return;
      }

      customUnits.push(unit);
    })

    while (childUnits.length > 0) {
      childUnits.forEach((cUnit, i) => {
        if (cUnit.templateType == TemplateType.固定單元) {
          fixedUnits.forEach(pUnit => {
            if (pUnit.id == cUnit.parent) {

              if (pUnit.children == null) {
                pUnit.children = [];
              }

              pUnit.children.push(cUnit)
              childUnits.splice(i, 1);
            }
          })
          return;
        }

        customUnits.forEach(pUnit => {
          if (pUnit.id == cUnit.parent) {

            if (pUnit.children == null) {
              pUnit.children = [];
            }

            pUnit.children.push(cUnit)
            childUnits.splice(i, 1);
          }
        })
      })
    }

    this._fixedUnits = fixedUnits;
    this._customUnits = customUnits;
  }

  public getUnitIcon(id: number) {
    switch (id) {
      case UnitID.帳戶管理:
        return 'people';
      case UnitID.單元管理:
        return 'dashboard';
      case UnitID.審核列表:
        return 'list';
      case UnitID.首頁設定:
        return 'settings';
      case UnitID.Footer設定:
        return 'settings';
      case UnitID.作品集:
        return 'photo';
      case UnitID.客戶服務:
        return 'work';
      case UnitID.常見問題:
        return 'question_answer';
      case UnitID.聯絡我們:
        return 'email';
      default:
        return 'settings';
    }
  }
}
