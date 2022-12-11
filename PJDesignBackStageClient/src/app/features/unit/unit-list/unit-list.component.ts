import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StageType, StatusCode } from 'src/app/shared/models/enums';
import { GetUnitsRequest, GetUnitsResponse } from 'src/app/shared/models/get-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { UnitDialogComponent } from '../feature-shared/components/unit-dialog/unit-dialog.component';

@Component({
  selector: 'app-unit-list',
  templateUrl: './unit-list.component.html',
  styleUrls: ['./unit-list.component.scss']
})
export class UnitListComponent extends ListBaseComponent implements OnInit {
  units = [
    {
      id: 1,
      name: '首頁',
      status: 1,
    },
    {
      id: 2,
      name: '關於我們',
      status: 1,
    },
    {
      id: 3,
      name: '作品集',
      status: 1,
    },
    {
      id: 4,
      name: '聯絡我們',
      status: 1,
    },
    {
      id: 5,
      name: '常見問題',
      status: 1,
    },
    {
      id: 6,
      name: '活動快訊',
      status: 1,
      children: [
        {
          id: 7,
          name: '節點一',
          status: 1,
        },
        {
          id: 8,
          name: '節點二',
          status: 1,
        },
        {
          id: 9,
          name: '節點三',
          status: 1,
        }
      ]
    },
    {
      id: 10,
      name: '知識部落格',
      status: 1,
    },
  ];

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
    request.stageType == StageType.前台;

    this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        return;
      }
    })
  }

  addChildNode(id?: number) {
    this.openDialog();
  }

  toggleNodeStatus(id: number) {

  }

  openDialog(): void {
    const dialogRef = this.dialog.open(UnitDialogComponent, {
      width: '300px',
      data: {
        id: 1,
        name: '測試',
        status: 1
      },
    });

    dialogRef.afterClosed().subscribe(res => {
      if (typeof res == undefined) { return; }
    });
  }

  drop(event: CdkDragDrop<string[]>, list: any) {
    moveItemInArray(list, event.previousIndex, event.currentIndex);
  }

  updateUnits() {
    this.httpService.post<ResponseBase<string>>('unit/updateUnits').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        return;
      }
    });
  }
}
