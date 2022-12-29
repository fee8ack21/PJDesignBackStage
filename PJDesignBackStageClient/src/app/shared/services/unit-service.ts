import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ResponseBase } from '../models/bases';
import { StageType, StatusCode, TemplateType, UnitID } from '../models/enums';
import { GetUnitsRequest, GetUnitsResponse, UnitList } from '../models/get-units';
import { AuthService } from './auth.service';
import { HttpService } from './http.service';
@Injectable()

export class UnitService {
  isBackStageUnitsInit$ = new BehaviorSubject<boolean>(false);
  units$ = new BehaviorSubject<{ fixedUnits: UnitList[], customUnits: UnitList[] }>({ fixedUnits: [], customUnits: [] });

  private _units: GetUnitsResponse[] | undefined;

  constructor(private httpService: HttpService, private authService: AuthService) { }

  getBackStageUnitsByGroupId(): void {
    let request = new GetUnitsRequest(StageType.後台, undefined, this.authService.getAdministrator()?.groupId);
    this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).subscribe(response => {
      if (response.statusCode == StatusCode.Success) {
        this._units = response.entries!;
        this.isBackStageUnitsInit$.next(true);
        this.units$.next(this._getFormattedUnits(response.entries! as UnitList[]))
      }
    });
  }

  getCurrentUnit(): { id: number, name: string } {
    if (this._units == null || this._units.length == 0) { return { id: -1, name: '' }; }

    const path = window.location.pathname + window.location.search;
    let filtededUnits = [];
    if (window.location.pathname.includes('type')) {
      if (window.location.pathname.includes('detail')) {
        const txt = `uid=${new URLSearchParams(window.location.search).get('uid')}`;
        filtededUnits = this._units.filter(x => x.backStageUrl != null ? x.backStageUrl.includes(txt) : false);
      } else {
        filtededUnits = this._units.filter(x => x.backStageUrl != null ? path == x.backStageUrl.trimStart().trim() ?? '' : false);
      }
    } else {
      filtededUnits = this._units.filter(x => x.backStageUrl != null ? path.includes(x.backStageUrl) : false);
    }

    if (filtededUnits.length == 0) { return { id: -1, name: '' }; }

    return { id: filtededUnits[0].id, name: filtededUnits[0].name };
  }

  private _getFormattedUnits(units: UnitList[]): { fixedUnits: UnitList[], customUnits: UnitList[] } {
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

    childUnits.forEach(c => {
      let hasMatched = false;
      fixedUnits.forEach(f => {
        if (f.id == c.parent) {
          if (f.children == null) {
            f.children = [c];
          } else {
            f.children.push(c);
          }
        }
      })

      if (hasMatched) { return; }
      customUnits.forEach(cs => {
        if (cs.id == c.parent) {
          if (cs.children == null) {
            cs.children = [c];
          } else {
            cs.children.push(c);
          }
        }
      })
    })

    return { fixedUnits, customUnits };
  }

  public clearUnits() {
    this._units = undefined;
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
