import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { GetQuestionsResponse } from '../feature-shared/models/get-questions';
import { QuestionListSearchParams } from '../feature-shared/models/question-list-search-params';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.scss']
})
export class QuestionListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetQuestionsResponse[] = [];
  displayedColumns: string[] = ['id', 'name', 'categories', 'createDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetQuestionsResponse>;
  searchParams = new QuestionListSearchParams();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
  ) {
    super();
  }

  ngOnInit(): void {
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
    return (this.searchParams.name == null || this.searchParams.name.trim().length == 0 || data.name.includes(this.searchParams.name.trim())) &&
      // (this.searchParams.categoryId == null || this.searchParams.categoryId == -1 || data.groupId == this.searchParams.groupId) &&
      (this.searchParams.startDt == null || new Date(data.createDt) >= this.searchParams.startDt) &&
      (this.searchParams.endDt == null || new Date(data.createDt) <= this.searchParams.endDt)
    // && (this.searchParams.isEnabled == null || this.searchParams.isEnabled == this.EnabledOptions.全部 || +data.isEnabled == this.searchParams.isEnabled)
  }

  resetSearchParams() {
    this.searchParams = new QuestionListSearchParams();
  }

  tableSortCb(state: Sort) {
    this.paginator.firstPage();
  }

  openCategoryDialog() {

  }
}
