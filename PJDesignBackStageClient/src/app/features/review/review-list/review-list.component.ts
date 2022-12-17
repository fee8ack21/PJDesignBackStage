import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { EditAndEnabledOptions, EditStatus } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { GetQuestionsResponse } from '../../question/feature-shared/models/get-questions';
import { QuestionListSearchParams } from '../../question/feature-shared/models/question-list-search-params';

@Component({
  selector: 'app-review-list',
  templateUrl: './review-list.component.html',
  styleUrls: ['./review-list.component.scss']
})
export class ReviewListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetQuestionsResponse[] = [];
  displayedColumns: string[] = ['id', 'title', 'categories', 'editDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetQuestionsResponse>;
  searchParams = new QuestionListSearchParams();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected unitService: UnitService,
    protected dialog: MatDialog) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit.subscribe(response => {
      this.setUnit();
    });
  }

  onSearch(): void {
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

  resetSearchParams(): void {
    this.searchParams = new QuestionListSearchParams();
  }

  tableSortCb(state: Sort): void {
    this.paginator.firstPage();
  }
}
