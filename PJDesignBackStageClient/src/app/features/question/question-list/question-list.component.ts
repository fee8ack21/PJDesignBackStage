import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { CategoryDialogComponent } from 'src/app/shared/components/category-dialog/category-dialog.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { CategoryDialogData } from 'src/app/shared/models/category-dialog-data';
import { EditAndEnabledOptions, EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetCategoriesByUnitId } from 'src/app/shared/models/get-categories-by-unit-id';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { GetQuestionsResponse } from '../feature-shared/models/get-questions';
import { QuestionListSearchParams } from '../feature-shared/models/question-list-search-params';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.scss']
})
export class QuestionListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetQuestionsResponse[] = [];
  displayedColumns: string[] = ['id', 'title', 'categories', 'editDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetQuestionsResponse>;
  searchParams = new QuestionListSearchParams();

  unitId: number;
  unitCategories: GetCategoriesByUnitId[] = [];
  unitEnabledCategories: GetCategoriesByUnitId[] = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    private unitService: UnitService,
    public dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.listenUnitService();
    this.getQuestions();
  }

  listenUnitService() {
    this.unitService.isBackStageUnitsInit.subscribe(response => {
      this.setUnitId();
      this.getCategories();
    });
  }

  setUnitId() {
    this.unitId = this.unitService.getCurrentUnit();
  }

  getCategories() {
    if (this.unitId == -1) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.unitCategories = response.entries ?? [];
      this.unitEnabledCategories = response.entries?.filter(x => x.isEnabled) ?? [];
    });
  }

  getQuestions() {
    this.httpService.get<ResponseBase<GetQuestionsResponse[]>>('question/getQuestions').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.rawListData = response.entries!;
      this.dataSource = new MatTableDataSource(this.rawListData);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });
  }

  onSearch() {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = new MatTableDataSource(newData);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchFilterFn(data: GetQuestionsResponse): boolean {
    return (this.searchParams.title == null || this.searchParams.title.trim().length == 0 || data.title.includes(this.searchParams.title.trim())) &&
      (this.searchParams.categoryId == null || this.searchParams.categoryId == -1 || (data.categories != null && data.categories.filter(x => x.id == this.searchParams.categoryId).length > 0)) &&
      (this.searchParams.startDt == null || new Date(data.editDt) >= this.searchParams.startDt) &&
      (this.searchParams.endDt == null || new Date(data.editDt) <= this.searchParams.endDt) &&
      (this.searchParams.editAndEnabledStatus == null ||
        this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.全部 ||
        (data.editStatus == null && +data.isEnabled == this.searchParams.editAndEnabledStatus) ||
        (data.editStatus == EditStatus.Review && this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.審核中) ||
        (data.editStatus == EditStatus.Reject && this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.駁回)
      )
  }

  resetSearchParams() {
    this.searchParams = new QuestionListSearchParams();
  }

  tableSortCb(state: Sort) {
    this.paginator.firstPage();
  }

  openCategoryDialog(isEdit = false) {
    let data = new CategoryDialogData();
    data.isEdit = isEdit;
    data.unitId = this.unitId;
    data.categories = this.unitCategories;

    const dialogRef = this.dialog.open(CategoryDialogComponent, {
      width: '474px',
      data: data
    });

    console.log('data', data);

    dialogRef.afterClosed().subscribe(doRefresh => {
      if (!doRefresh) { return; }

      this.getCategories();
    });
  }
}
