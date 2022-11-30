import { Injectable } from '@angular/core';
import { ResponseBase } from '../models/bases';
import { StatusCode, TemplateType, UnitID } from '../models/enums';
import { GetUnitsResponse } from '../models/get-units';
import { HttpService } from './http.service';
@Injectable()

export class UnitService {
  private _units: GetUnitsResponse[];
  private _fixedUnits: GetUnitsResponse[] = [];
  private _customUnits: GetUnitsResponse[] = [];

  constructor(private httpService: HttpService) { }

  async getUnits() {
    if (this._units != undefined) { return [this._fixedUnits, this._customUnits] };

    const response = await this.httpService.get<ResponseBase<GetUnitsResponse[]>>('unit/getUnits').toPromise();
    if (response.statusCode == StatusCode.Success) {
      this._units = response.entries!;
      this._setFormattedUnits(response.entries!)

      return [this._fixedUnits, this._customUnits];
    }

    return [[], []];
  }

  private _setFormattedUnits(units: GetUnitsResponse[]) {
    let fixedUnits: GetUnitsResponse[] = []
    let customUnits: GetUnitsResponse[] = []
    let childUnits: GetUnitsResponse[] = []

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
