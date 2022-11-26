import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { UnitDialogComponent } from '../feature-shared/components/unit-dialog/unit-dialog.component';
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

  constructor(public dialog: MatDialog) {
    this.dataSource.data = TREE_DATA;
  }

  ngOnInit(): void {
  }

  hasChild = (_: number, node: UnitFlatNode) => node.expandable;

  addChildNode(id?: number) {
    this.openDialog();
  }

  toggleNodeStatus(id: number) {

  }

  openDialog(): void {
    const dialogRef = this.dialog.open(UnitDialogComponent, {
      width: '250px',
      data: {
        id: 1,
        name: '測試',
        status: 1
      },
    });

    dialogRef.afterClosed().subscribe(res => {
      if (typeof res == undefined) { return; }
      console.log(res)
    });
  }
}
