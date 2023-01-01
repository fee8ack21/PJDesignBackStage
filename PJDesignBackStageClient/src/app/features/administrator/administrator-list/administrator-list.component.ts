import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { Group, StageType, StatusCode } from 'src/app/shared/models/enums';
import { GetUnitsRequest, GetUnitsResponse } from 'src/app/shared/models/get-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { UnitService } from 'src/app/shared/services/unit-service';
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
    protected httpService: HttpService,
    protected unitService: UnitService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog,
    private spinnerService: SpinnerService) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(() => { this.setUnit(); });
    this.fetchPageData();
  }

  async fetchPageData() {
    this.spinnerService.isShow$.next(true);

    await Promise.all([
      this.getAdministratorsPromise(),
      this.getGroupsPromise()
    ]).then(([administratorsResponse, groupsResponse]) => {
      this.handleAdministratorsResponse(administratorsResponse);
      this.handleGroupsResponse(groupsResponse);

      this.spinnerService.isShow$.next(false);
    });
  }

  getAdministratorsPromise() {
    return this.httpService.get<ResponseBase<GetAdministratorsResponse[]>>('administrator/getAdministrators').toPromise();
  }
  handleAdministratorsResponse(response: ResponseBase<GetAdministratorsResponse[]>) {
    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    } else {
      this.rawListData = response.entries!;
      this.dataSource = this.createDataSource<GetAdministratorsResponse>(this.rawListData, this.sort, this.paginator);
    }
  }

  getGroupsPromise() {
    return this.httpService.get<ResponseBase<GetGroupsResponse[]>>('administrator/getGroups').toPromise();
  }
  handleGroupsResponse(response: ResponseBase<GetGroupsResponse[]>) {
    if (response.statusCode == StatusCode.Fail) {
      this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
      return;
    }
    this.groups = response.entries!;
  }

  getBackStageUnitsPromise() {
    let request = new GetUnitsRequest(StageType.後台);
    return this.httpService.post<ResponseBase<GetUnitsResponse[]>>('unit/getUnits', request).toPromise();
  }

  getRightsPromise() {
    return this.httpService.get<ResponseBase<GetRightsResponse[]>>('administrator/getRights').toPromise();
  }

  async getDialogData() {
    let units: GetUnitsResponse[] = [];
    let rights: GetRightsResponse[] = [];

    await Promise.all([
      this.getBackStageUnitsPromise(),
      this.getRightsPromise()
    ]).then(([unitsResponse, rightsResponse]) => {
      if (unitsResponse.statusCode == StatusCode.Success) {
        unitsResponse.entries!.forEach(unit => {
          units.push({ ...unit } as GetUnitsResponse);
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

    dialogRef.afterClosed().subscribe(async doRefresh => {
      if (!doRefresh) { return; }
      this.handleGroupsResponse(await this.getGroupsPromise());
    });
  }

  onSearch() {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = this.createDataSource<GetAdministratorsResponse>(newData, this.sort, this.paginator);
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
