import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StageType, StatusCode } from 'src/app/shared/models/enums';
import { GetUnitsRequest, GetUnitsResponse, UnitList } from 'src/app/shared/models/get-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { UnitDialogComponent } from './feature-shared/components/unit-dialog/unit-dialog.component';
import { UnitDialogData } from './feature-shared/models/unit-dialog-data';
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
    protected unitService: UnitService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit.subscribe(() => { this.setUnit(); })
    this.getFrontStageUnits();
  }

  getFrontStageUnits() {
    let request = new GetUnitsRequest();
    request.stageType = StageType.前台;

    this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.formatUnits(response.entries as UnitList[]);
    })
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
      if (a.sort === b.sort) {
        return 0;
      }
      if (a.sort === null || a.sort === undefined) {
        return 1;
      }
      if (b.sort === null || b.sort === undefined) {
        return -1;
      }
      return a.sort < b.sort ? -1 : 1;
    };
  }

  toggleNodeStatus(id: number) {
    this.units.forEach(u => {
      if (u.id == id) {
        u.isEnabled = !u.isEnabled;
        return;
      }

      if (u.children != null) {
        u.children.forEach(c => {
          if (c.id == id) {
            c.isEnabled = !c.isEnabled;
            return;
          }
        })
      }
    })
  }

  openDialog(unit?: UnitList, parent?: number): void {
    let data: UnitDialogData = { unit, parent };
    const dialogRef = this.dialog.open(UnitDialogComponent, {
      width: '300px',
      data: data
    });

    dialogRef.afterClosed().subscribe(doRefresh => {
      if (doRefresh) { this.getFrontStageUnits(); }
    });
  }

  drop(event: CdkDragDrop<string[]>, list: any) {
    moveItemInArray(list, event.previousIndex, event.currentIndex);
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
