import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { GetPortfoliosResponse } from '../feature-shared/models/get-portfolios';
import { PortfolioListSearchParams } from '../feature-shared/models/portfolio-list-search-params';

@Component({
  selector: 'app-portfolio-list',
  templateUrl: './portfolio-list.component.html',
  styleUrls: ['./portfolio-list.component.scss']
})
export class PortfolioListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetPortfoliosResponse[] = [];
  displayedColumns: string[] = ['id', 'title', 'categories', 'date', 'editDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetPortfoliosResponse>;
  searchParams = new PortfolioListSearchParams();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog,
    protected unitService: UnitService) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.getPortfolios();

    this.unitService.isBackStageUnitsInit.subscribe(() => {
      this.setUnit();
      this.getCategories();
    })
  }

  getPortfolios(): void {
    this.httpService.get<ResponseBase<GetPortfoliosResponse[]>>('portfolio/getPortfolios').subscribe(response => {
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

  onSearch(): void {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = new MatTableDataSource(newData);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchFilterFn(data: GetPortfoliosResponse): boolean {
    // return (this.searchParams.title == null || this.searchParams.title.trim().length == 0 || data.title.includes(this.searchParams.title.trim())) &&
    //   (this.searchParams.categoryId == null || this.searchParams.categoryId == -1 || (data.categories != null && data.categories.filter(x => x.id == this.searchParams.categoryId).length > 0)) &&
    //   (this.searchParams.startDt == null || new Date(data.editDt) >= this.searchParams.startDt) &&
    //   (this.searchParams.endDt == null || new Date(data.editDt) <= this.searchParams.endDt) &&
    //   (this.searchParams.editAndEnabledStatus == null ||
    //     this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.全部 ||
    //     (data.editStatus == null && +data.isEnabled == this.searchParams.editAndEnabledStatus) ||
    //     (data.editStatus == EditStatus.Review && this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.審核中) ||
    //     (data.editStatus == EditStatus.Reject && this.searchParams.editAndEnabledStatus == EditAndEnabledOptions.駁回)
    //   )
    return true;
  }

  resetSearchParams(): void {
    this.searchParams = new PortfolioListSearchParams();
  }

  tableSortCb(state: Sort): void {
    this.paginator.firstPage();
  }
}
