import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ResponseBase } from '../models/bases';
import { StageType, StatusCode, TemplateType, UnitID } from '../models/enums';
import { GetUnitsRequest, GetUnitsResponse, UnitList } from '../models/get-units';
import { AuthService } from './auth.service';
import { HttpService } from './http.service';
@Injectable()

export class UnitService {
  isBackStageUnitsInit = new BehaviorSubject<boolean>(false);
  private _units: GetUnitsResponse[];
  private _fixedUnits: UnitList[] = [];
  private _customUnits: UnitList[] = [];

  constructor(private httpService: HttpService, private authService: AuthService) { }

  async getBackStageUnitsByGroupId(): Promise<{ fixedUnits: UnitList[], customUnits: UnitList[] }> {
    if (this._units != undefined) { return { fixedUnits: this._fixedUnits, customUnits: this._customUnits } };

    let request = new GetUnitsRequest();
    request.groupId = this.authService.getAdministrator()?.groupId;
    request.stageType = StageType.後台;

    const response = await this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
    if (response.statusCode == StatusCode.Success) {
      this._units = response.entries!;
      this._setFormattedUnits(response.entries! as UnitList[])

      this.isBackStageUnitsInit.next(true);
      return { fixedUnits: this._fixedUnits, customUnits: this._customUnits };
    }

    return { fixedUnits: [], customUnits: [] };
  }

  getCurrentUnit(): { id: number, name: string } {
    if (this._units == null || this._units.length == 0) { return { id: -1, name: '' }; }

    const path = window.location.pathname;
    const filtededUnits = this._units.filter(x => x.backStageUrl != null ? path.includes(x.backStageUrl ?? '') : false);

    if (filtededUnits.length == 0) { return { id: -1, name: '' }; }

    return { id: filtededUnits[0].id, name: filtededUnits[0].name };
  }

  private _setFormattedUnits(units: UnitList[]) {
    let fixedUnits: UnitList[] = []
    let customUnits: UnitList[] = []
    let childUnits: UnitList[] = []

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
