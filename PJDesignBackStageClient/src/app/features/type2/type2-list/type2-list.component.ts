import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { EditAndEnabledOptions, EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { GetType2ContentsResponse } from '../feature-shared/models/get-type2-contents';
import { Type2ListSearchParams } from '../feature-shared/models/type2-list-search-params';

@Component({
  selector: 'app-type2-list',
  templateUrl: './type2-list.component.html',
  styleUrls: ['./type2-list.component.scss']
})
export class Type2ListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetType2ContentsResponse[] = [];
  displayedColumns: string[] = ['id', 'title', 'categories', 'editDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetType2ContentsResponse>;
  searchParams = new Type2ListSearchParams();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private router: Router,
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected unitService: UnitService,
    protected dialog: MatDialog) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

    this.unitService.isBackStageUnitsInit.subscribe(response => {
      this.setUnit();
      this.getCategories();
      this.getType2Contents();
    });
  }

  getType2Contents(): void {
    if (!this.isUnitInit()) { return; }

    this.httpService.get<ResponseBase<GetType2ContentsResponse[]>>(`type2/getType2ContentsByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.rawListData = response.entries!;
      this.dataSource = this.createDataSource<GetType2ContentsResponse>(this.rawListData, this.sort, this.paginator);
    });
  }

  onSearch(): void {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = this.createDataSource<GetType2ContentsResponse>(newData, this.sort, this.paginator);
  }

  onSearchFilterFn(data: GetType2ContentsResponse): boolean {
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
    this.searchParams = new Type2ListSearchParams();
  }

  tableSortCb(state: Sort): void {
    this.paginator.firstPage();
  }
}
