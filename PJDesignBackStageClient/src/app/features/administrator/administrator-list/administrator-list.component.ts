import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { GetAdministratorsResponse } from '../feature-shared/models/get-administrators';
import { GetGroupsResponse } from '../feature-shared/models/get-groups';

@Component({
  selector: 'app-administrator-list',
  templateUrl: './administrator-list.component.html',
  styleUrls: ['./administrator-list.component.scss']
})

export class AdministratorListComponent extends ListBaseComponent implements OnInit {
  displayedColumns: string[] = ['id', 'account', 'name', 'createDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetAdministratorsResponse>;
  groups: GetGroupsResponse[] = [];

  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    private _liveAnnouncer: LiveAnnouncer) {
    super();
  }

  ngOnInit(): void {
    this.getGroups();
    this.getAdministrators();
  }

  todo() {
    // forkJoin();
  }

  getAdministrators() {
    this.httpService.get<ResponseBase<GetAdministratorsResponse[]>>('administrator/getAdministrators').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar('請求錯誤');
        return;
      }
      this.dataSource = new MatTableDataSource(response.entries);
      this.dataSource.sort = this.sort;
    });
  }

  getGroups() {
    this.httpService.get<ResponseBase<GetGroupsResponse[]>>('administrator/getGroups').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar('請求錯誤');
        return;
      }

      this.groups = response.entries!;
    });
  }

  /** Announce the change in sort state for assistive technology. */
  announceSortChange(sortState: Sort) {
    // This example uses English messages. If your application supports
    // multiple language, you would internationalize these strings.
    // Furthermore, you can customize the message to add additional
    // details about the values being sorted.
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }
}
