import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StageType, StatusCode } from 'src/app/shared/models/enums';
import { GetUnitsRequest, GetUnitsResponse, UnitList } from 'src/app/shared/models/get-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { UnitDialogComponent } from './feature-shared/components/unit-dialog/unit-dialog.component';
import { UpdateUnitsSortRequest } from './feature-shared/models/update-units-sort';

@Component({
  selector: 'app-unit',
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.scss']
})
export class UnitComponent extends ListBaseComponent implements OnInit {
  units: UnitList[] = [];

  constructor(
    protected httpService: HttpService,
    private spinnerService: SpinnerService,
    protected unitService: UnitService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(() => { this.setUnit(); })
    this.fetchPageData();
  }

  async fetchPageData() {
    this.spinnerService.isShow$.next(true);

    await Promise.all([
      this.getFrontStageUnitsPromise(),
    ]).then(([unitsResponse]) => {
      this.handleUnitsResponse(unitsResponse);

      this.spinnerService.isShow$.next(false);
    });
  }

  getFrontStageUnitsPromise() {
    let request = new GetUnitsRequest(StageType.前台);
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }
  handleUnitsResponse(response: ResponseBase<GetUnitsResponse[]>) {
    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }

    this.formatUnits(response.entries as UnitList[]);
  }

  formatUnits(units: UnitList[]) {
    let parent = units.filter(x => x.parent == null);
    let children = units.filter(x => x.parent != null);

    parent = parent.sort(this.sortFn());

    children.forEach(c => {
      parent.forEach(p => {
        if (p.id == c.parent) {
          if (p.children == null) {
            p.children = [c]
          } else {
            p.children.push(c);
          }
        }
      })
    })

    parent.forEach(p => {
      if (p.children != null && p.children.length > 0) {
        p.children = p.children.sort(this.sortFn());
      }
    })

    this.units = parent;
  }

  sortFn() {
    return function (a: GetUnitsResponse, b: GetUnitsResponse) {
      if (a.sort === b.sort) { return 0; }

      if (a.sort === null || a.sort === undefined) { return 1; }

      if (b.sort === null || b.sort === undefined) { return -1; }

      return a.sort < b.sort ? -1 : 1;
    };
  }

  openDialog(unit?: UnitList, parent?: number): void {
    const dialogRef = this.dialog.open(UnitDialogComponent, {
      width: '300px',
      data: { unit, parent }
    });

    dialogRef.afterClosed().subscribe(async doRefresh => {
      if (doRefresh) {
        this.handleUnitsResponse(await this.getFrontStageUnitsPromise());
      }
    });
  }

  getUpdateUnitsSortRequest(): UpdateUnitsSortRequest[] {
    let temp: UpdateUnitsSortRequest[] = [];

    this.units.forEach((unit, index) => {
      temp.push(new UpdateUnitsSortRequest(unit.id, index + 1));

      if (unit.children != null) {
        unit.children.forEach((cUnit, cIndex) => {
          temp.push(new UpdateUnitsSortRequest(cUnit.id, cIndex + 1));
        })
      }
    })

    return temp;
  }

  updateUnitsSort() {
    this.httpService.post<ResponseBase<string>>('unit/updateUnitsSort', this.getUpdateUnitsSortRequest()).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
    });
  }
}
