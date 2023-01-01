import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseBase } from '../../models/bases';
import { Category } from '../../models/category';
import { CategoryDialogData } from '../../models/category-dialog-data';
import { EnabledOptions, EditStatus, EditAndEnabledOptions, StatusCode } from '../../models/enums';
import { GetCategoriesByUnitId } from '../../models/get-categories-by-unit-id';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { CategoryDialogComponent } from '../category-dialog/category-dialog.component';
import { BaseComponent } from './base.component';

@Injectable()
export abstract class ListBaseComponent extends BaseComponent {
  readonly enabledOptions = [{ name: '全部', value: EnabledOptions.全部 }, { name: '啟用', value: EnabledOptions.啟用 }, { name: '停用', value: EnabledOptions.停用 }]
  readonly editAndEnabledOptions = [{ name: '全部', value: EditAndEnabledOptions.全部 }, { name: '啟用', value: EditAndEnabledOptions.啟用 }, { name: '停用', value: EditAndEnabledOptions.停用 }, { name: '審核中', value: EditAndEnabledOptions.審核中 }, { name: '駁回', value: EditAndEnabledOptions.駁回 }]

  unitCategories: GetCategoriesByUnitId[] = [];

  constructor(protected unitService: UnitService, protected httpService: HttpService, protected snackBarService: SnackBarService, protected dialog: MatDialog) {
    super(unitService);
  }

  public get EnabledOptions(): typeof EnabledOptions {
    return EnabledOptions;
  }

  getEnabledOptionName(value: boolean | number): string {
    return value ? '啟用' : '停用';
  }

  getEditAndEnabledStatusName(status: boolean | number): string {
    if (typeof status == 'boolean') {
      if (status) { return '啟用'; }

      return '停用';
    }

    switch (status) {
      case EditStatus.Review:
        return '審核中';
      case EditStatus.Reject:
        return '駁回';
      default:
        return '審核中';
    }
  }

  getCategoryTooltip(categories: Category[]): string {
    let txt = '';
    categories.forEach((item, i) => {
      if (i != categories.length - 1) {
        txt += `${item.name}, `
        return;
      }
      txt += item.name;
    })

    return txt;
  }

  getCategoriesPromise() {
    if (!this.isUnitInit()) { return; }
    return this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unit.id}`).toPromise();
  }
  handleCategoriesResponse(response: ResponseBase<GetCategoriesByUnitId[]> | undefined) {
    if (response == undefined) { return; }

    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }

    this.unitCategories = response.entries ?? [];
  }

  openCategoryDialog(isEdit = false): void {
    const dialogRef = this.dialog.open(CategoryDialogComponent, {
      width: '474px',
      data: new CategoryDialogData(this.unit.id, isEdit, this.unitCategories)
    });

    dialogRef.afterClosed().subscribe(async doRefresh => {
      if (!doRefresh) { return; }
      this.handleCategoriesResponse(await this.getCategoriesPromise());
    });
  }

  createDataSource<T>(data: T[], sort: MatSort, paginator: MatPaginator): MatTableDataSource<T> {
    let source = new MatTableDataSource<T>(data);
    source.sort = sort;
    source.paginator = paginator;

    return source;
  }
}
