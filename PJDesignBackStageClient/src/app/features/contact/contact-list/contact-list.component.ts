import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ListBaseComponent } from 'src/app/shared/components/base/list-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ContactDialogComponent } from '../feature-shared/components/contact-dialog/contact-dialog.component';
import { ContactDialogData } from '../feature-shared/models/contact-dialog-data';
import { ContactListSearchParams } from '../feature-shared/models/contact-list-search-params';
import { GetContactsResponse } from '../feature-shared/models/get-contacts';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent extends ListBaseComponent implements OnInit {
  rawListData: GetContactsResponse[] = [];
  displayedColumns: string[] = ['id', 'name', 'email', 'phone', 'createDt', 'tool'];
  dataSource: MatTableDataSource<GetContactsResponse>;
  searchParams = new ContactListSearchParams();

  administrator: { id: number, name: string } | null;
  settingEditorId?: number;
  settingStatus?: EditStatus;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    protected unitService: UnitService,
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog) {
    super(unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit.subscribe(() => { this.setUnit(); })
    this.getContacts();
  }

  getContacts() {
    this.httpService.get<ResponseBase<GetContactsResponse[]>>('contact/getContacts').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.rawListData = response.entries!;
      this.dataSource = this.createDataSource<GetContactsResponse>(this.rawListData, this.sort, this.paginator);
    });
  }

  async openDialog(index: number): Promise<void> {
    const _data = this.dataSource.data[index];

    this.dialog.open(ContactDialogComponent, {
      width: '474px',
      data: new ContactDialogData(_data.name, _data.content, _data.createDt)
    });
  }

  onSearch() {
    const newData = this.rawListData.filter(data => this.onSearchFilterFn(data));
    this.dataSource = this.createDataSource<GetContactsResponse>(newData, this.sort, this.paginator);
  }

  onSearchFilterFn(data: GetContactsResponse): boolean {
    return (this.searchParams.name == null || this.searchParams.name.trim().length == 0 || data.name.includes(this.searchParams.name.trim())) &&
      (this.searchParams.email == null || this.searchParams.email.trim().length == 0 || data.email.includes(this.searchParams.email.trim())) &&
      (this.searchParams.phone == null || this.searchParams.phone.trim().length == 0 || data.phone.includes(this.searchParams.phone.trim())) &&
      (this.searchParams.startDt == null || new Date(data.createDt) >= this.searchParams.startDt) &&
      (this.searchParams.endDt == null || new Date(data.createDt) <= this.searchParams.endDt)
  }

  resetSearchParams() {
    this.searchParams = new ContactListSearchParams();
  }

  tableSortCb(state: Sort) {
    this.paginator.firstPage();
  }
}
