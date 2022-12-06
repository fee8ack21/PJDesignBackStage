import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode, TemplateType } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitDialogComponent } from '../feature-shared/components/unit-dialog/unit-dialog.component';
import { GetFrontStageUnits } from '../feature-shared/models/get-front-stage-units';
import { UnitFlatNode } from '../feature-shared/models/unit-flat-node';
import { UnitNode } from '../feature-shared/models/unit-node';

let TREE_DATA: UnitNode[] = [
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

@Component({
  selector: 'app-unit-list',
  templateUrl: './unit-list.component.html',
  styleUrls: ['./unit-list.component.scss']
})
export class UnitListComponent implements OnInit {
  private _transformer = (node: UnitNode, level: number) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      id: node.id,
      status: node.status,
      name: node.name,
      level: level,
    };
  };

  treeControl = new FlatTreeControl<UnitFlatNode>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children,
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    public dialog: MatDialog) {
    this.dataSource.data = TREE_DATA;
  }

  ngOnInit(): void {
    this.getFrontStageUnits();
  }

  getFrontStageUnits() {
    this.httpService.get<ResponseBase<GetFrontStageUnits>>('unit/getFrontStageUnits').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        return;
      }
    })
  }

  hasChild = (_: number, node: UnitFlatNode) => node.expandable;

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
}
