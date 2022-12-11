import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { ResponseBase } from '../../models/bases';
import { Category } from '../../models/category';
import { CategoryDialogData } from '../../models/category-dialog-data';
import { EnabledOptions, FormControlErrorType, PageStatus, EditStatus, EditAndEnabledOptions, StatusCode } from '../../models/enums';
import { GetCategoriesByUnitId } from '../../models/get-categories-by-unit-id';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { CategoryDialogComponent } from '../category-dialog/category-dialog.component';

@Injectable()
export abstract class ListBaseComponent {
  readonly enabledOptions = [{ name: '全部', value: EnabledOptions.全部 }, { name: '啟用', value: EnabledOptions.啟用 }, { name: '停用', value: EnabledOptions.停用 }]
  readonly editAndEnabledOptions = [{ name: '全部', value: EditAndEnabledOptions.全部 }, { name: '啟用', value: EditAndEnabledOptions.啟用 }, { name: '停用', value: EditAndEnabledOptions.停用 }, { name: '審核中', value: EditAndEnabledOptions.審核中 }, { name: '駁回', value: EditAndEnabledOptions.駁回 }]

  unitCategories: GetCategoriesByUnitId[] = [];
  unitEnabledCategories: GetCategoriesByUnitId[] = [];

  unit: { id: number, name: string }

  constructor(protected unitService: UnitService, protected httpService: HttpService, protected snackBarService: SnackBarService, protected dialog: MatDialog) { }

  public get EditStatus(): typeof EditStatus {
    return EditStatus;
  }

  public get EnabledOptions(): typeof EnabledOptions {
    return EnabledOptions;
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  setUnit() {
    this.unit = this.unitService.getCurrentUnit();
  }

  getPageStatusName(status: number) {
    switch (status) {
      case PageStatus.Create:
        return '新增';
      case PageStatus.Edit:
        return '編輯';
      case PageStatus.Review:
        return '審核'
      default:
        return '新增'
    }
  }

  getEnabledOptionName(value: boolean | number) {
    return value ? '啟用' : '停用';
  }

  getEditAndEnabledStatusName(status: boolean | number) {
    if (typeof status == 'boolean') {
      if (status) {
        return '啟用';
      }

      return '停用'
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

  getCategoryTooltip(categories: Category[]) {
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

  getCategories() {
    if (this.unit.id == -1) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.unitCategories = response.entries ?? [];
      this.unitEnabledCategories = response.entries?.filter(x => x.isEnabled) ?? [];
    });
  }

  openCategoryDialog(isEdit = false) {
    let data = new CategoryDialogData();
    data.isEdit = isEdit;
    data.unitId = this.unit.id;
    data.categories = this.unitCategories;

    const dialogRef = this.dialog.open(CategoryDialogComponent, {
      width: '474px',
      data: data
    });

    dialogRef.afterClosed().subscribe(doRefresh => {
      if (!doRefresh) { return; }

      this.getCategories();
    });
  }
}
