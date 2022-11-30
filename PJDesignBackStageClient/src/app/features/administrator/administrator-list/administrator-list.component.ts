import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { StatusCode } from 'src/app/shared/models/enums';
import { GetBackStageUnitsResponse } from 'src/app/shared/models/get-back-stage-units';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { GroupDialogComponent } from '../feature-shared/components/group-dialog/group-dialog.component';
import { CreateGroupRequest, CreateGroupResponse } from '../feature-shared/models/create-group';
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
  displayedColumns: string[] = ['id', 'account', 'name', 'createDt', 'isEnabled', 'tool'];
  dataSource: MatTableDataSource<GetAdministratorsResponse>;
  groups: GetGroupsResponse[] = [];

  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    private _liveAnnouncer: LiveAnnouncer,
    public dialog: MatDialog) {
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
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
      this.dataSource = new MatTableDataSource(response.entries);
      this.dataSource.sort = this.sort;
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

  getBackStageUnits() {
    return this.httpService.get<ResponseBase<GetBackStageUnitsResponse[]>>('unit/getBackStageUnits').toPromise();
  }

  getRights() {
    return this.httpService.get<ResponseBase<GetRightsResponse[]>>('administrator/getRights').toPromise();
  }

  async openDialog(): Promise<void> {
    let units: GetBackStageUnitsResponse[] = [];
    let rights: GetRightsResponse[] = [];

    await Promise.all([
      this.getBackStageUnits(),
      this.getRights()
    ]).then(([unitsResponse, rightsResponse]) => {
      if (unitsResponse.statusCode == StatusCode.Success) { units = unitsResponse.entries! };
      if (rightsResponse.statusCode == StatusCode.Success) { rights = rightsResponse.entries! };
    });

    const data: GroupDialogData = { name: '', units: units, rights: rights };
    const dialogRef = this.dialog.open(GroupDialogComponent, {
      width: '474px',
      data: data
    });

    dialogRef.afterClosed().subscribe(response => {
      if (typeof response == undefined) { return; }
      console.log(response)
      this.CreateGroup(response.name);
    });
  }

  CreateGroup(name: string) {
    const request = new CreateGroupRequest(name);

    this.httpService.post<ResponseBase<CreateGroupResponse>>('administrator/createGroup', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }
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
