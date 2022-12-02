import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { Group, StatusCode } from 'src/app/shared/models/enums';
import { GetBackStageUnitsResponse } from 'src/app/shared/models/get-back-stage-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { GroupDialogComponent } from '../feature-shared/components/group-dialog/group-dialog.component';
import { AdministratorListSearchParams } from '../feature-shared/models/administrator-list-search-params';
import { GetAdministratorsResponse } from '../feature-shared/models/get-administrators';
import { GetGroupsResponse } from '../feature-shared/models/get-groups';
import { GetRightsResponse } from '../feature-shared/models/get-rights';
import { GroupDialogData } from '../feature-shared/models/group-dialog-data';

@Component({
  selector: 'app-administrator-list',
  templateUrl: './administrator-list.component.html',
  styleUrls: ['./administrator-list.component.scss']
})

export class AdministratorListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetAdministratorsResponse[] = [];
  displayedColumns: string[] = ['id', 'account', 'name', 'groupName', 'createDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetAdministratorsResponse>;
  groups: GetGroupsResponse[] = [];
  searchParams = new AdministratorListSearchParams();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    public dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.getGroups();
    this.getAdministrators();
  }

  getAdministrators() {
    this.httpService.get<ResponseBase<GetAdministratorsResponse[]>>('administrator/getAdministrators').subscribe(response => {
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

  getGroups() {
    this.httpService.get<ResponseBase<GetGroupsResponse[]>>('administrator/getGroups').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
      this.groups = response.entries!;
    });
  }

  getBackStageUnitsPromise() {
    return this.httpService.get<ResponseBase<GetBackStageUnitsResponse[]>>('unit/getBackStageUnits').toPromise();
  }

  getRightsPromise() {
    return this.httpService.get<ResponseBase<GetRightsResponse[]>>('administrator/getRights').toPromise();
  }

  async getDialogData() {
    let units: GetBackStageUnitsResponse[] = [];
    let rights: GetRightsResponse[] = [];

    await Promise.all([
      this.getBackStageUnitsPromise(),
      this.getRightsPromise()
    ]).then(([unitsResponse, rightsResponse]) => {
      if (unitsResponse.statusCode == StatusCode.Success) {
        unitsResponse.entries!.forEach(unit => {
          let temp = new GetBackStageUnitsResponse();
          temp = { ...unit };
          units.push(temp);
        })
      };
      if (rightsResponse.statusCode == StatusCode.Success) { rights = rightsResponse.entries! };
    });

    return { units: units, rights: rights }
  }

  async openDialog(isEdit = false): Promise<void> {
    const dialogData = await this.getDialogData();
    const filteredGroups = this.groups.filter(group => group.id != Group.系統管理員);
    const data: GroupDialogData = {
      id: isEdit && filteredGroups != null && filteredGroups.length > 0 ? filteredGroups[0].id : undefined,
      name: isEdit && filteredGroups != null && filteredGroups.length > 0 ? filteredGroups[0].name : undefined,
      units: dialogData.units,
      rights: dialogData.rights,
      groups: filteredGroups, isEdit
    };
    const dialogRef = this.dialog.open(GroupDialogComponent, {
      width: '474px',
      data: data
    });

    dialogRef.afterClosed().subscribe(doRefresh => {
      if (!doRefresh) { return; }
      this.getGroups();
    });
  }

  onSearch() {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = new MatTableDataSource(newData);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchFilterFn(data: GetAdministratorsResponse): boolean {
    return (this.searchParams.name == null || this.searchParams.name.trim().length == 0 || data.name.includes(this.searchParams.name.trim())) &&
      (this.searchParams.groupId == null || this.searchParams.groupId == -1 || data.groupId == this.searchParams.groupId) &&
      (this.searchParams.startDt == null || new Date(data.createDt) >= this.searchParams.startDt) &&
      (this.searchParams.endDt == null || new Date(data.createDt) <= this.searchParams.endDt) &&
      (this.searchParams.isEnabled == null || this.searchParams.isEnabled == this.EnabledOptions.全部 || +data.isEnabled == this.searchParams.isEnabled)
  }

  resetSearchParams() {
    this.searchParams = new AdministratorListSearchParams();
  }

  tableSortCb(state: Sort) {
    this.paginator.firstPage();
  }
}
