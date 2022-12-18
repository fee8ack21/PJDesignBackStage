import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { EditAndEnabledOptions, EditStatus, StatusCode, UnitID } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { GetQuestionsResponse } from '../../question/feature-shared/models/get-questions';
import { QuestionListSearchParams } from '../../question/feature-shared/models/question-list-search-params';
import { GetReviewsResponse } from '../feature-shared/models/get-reviews';
import { ReviewListSearchParams } from '../feature-shared/models/review-list-search-params';

@Component({
  selector: 'app-review-list',
  templateUrl: './review-list.component.html',
  styleUrls: ['./review-list.component.scss']
})
export class ReviewListComponent extends ListBaseComponent implements OnInit {
  unitOptions: { id: number, name: string }[] = [];
  rawListData: GetReviewsResponse[] = [];
  displayedColumns: string[] = ['id', 'unitName', 'title', 'editorName', 'editDt', 'tool'];
  dataSource: MatTableDataSource<GetReviewsResponse>;
  searchParams = new ReviewListSearchParams();

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
      this.getReviews();
    });
  }

  getReviews() {
    if (this.rawListData.length > 0) { return; }

    this.httpService.get<ResponseBase<GetReviewsResponse[]>>('review/getReviews').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.rawListData = response.entries!;
      this.dataSource = new MatTableDataSource(this.rawListData);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;

      this.setUnitOptions(this.rawListData);
    })
  }

  setUnitOptions(data: GetReviewsResponse[]) {
    this.unitOptions = [];

    let unitDict: any = {};
    data.forEach(item => {
      if (item.unitId in unitDict) {
        return;
      }

      unitDict[item.unitId.toString()] = item.unitName;
      this.unitOptions.push({ id: item.unitId, name: item.unitName });
    })
  }

  getLink(url: string | null | undefined) {
    if (url == null || url == undefined) { return url; }

    return url.split('?')[0];
  }

  getQueryParams(url: string | null | undefined) {
    if (url == null || url == undefined || url.split('?').length == 1) { return {}; }

    const params = new URLSearchParams(url.split('?')[1]);
    return Object.fromEntries(params.entries());
  }

  onSearch(): void {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = new MatTableDataSource(newData);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchFilterFn(data: GetReviewsResponse): boolean {
    return (this.searchParams.unitId == null || this.searchParams.unitId == -1 || data.unitId == this.searchParams.unitId) &&
      (this.searchParams.title == null || this.searchParams.title.trim().length == 0 || data.title!.includes(this.searchParams.title.trim())) &&
      (this.searchParams.editorName == null || this.searchParams.editorName.trim().length == 0 || data.editorName.includes(this.searchParams.editorName.trim())) &&
      (this.searchParams.startDt == null || new Date(data.editDt) >= this.searchParams.startDt) &&
      (this.searchParams.endDt == null || new Date(data.editDt) <= this.searchParams.endDt)
  }

  resetSearchParams(): void {
    this.searchParams = new ReviewListSearchParams();
  }

  tableSortCb(state: Sort): void {
    this.paginator.firstPage();
  }
}
