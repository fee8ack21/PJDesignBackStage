import { Inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute } from '@angular/router';
import { ResponseBase } from '../../models/bases';
import { Category } from '../../models/category';
import { EditStatus, PageStatus, StatusCode } from '../../models/enums';
import { GetCategoriesByUnitId } from '../../models/get-categories-by-unit-id';
import { ReviewNote } from '../../models/review-note';
import { ReviewNoteDialogData } from '../../models/review-note-dialog-data';
import { AuthService } from '../../services/auth.service';
import { HttpService } from '../../services/http.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { UnitService } from '../../services/unit-service';
import { ReviewNoteDialogComponent } from '../review-note-dialog/review-note-dialog.component';
import { BaseComponent } from './base.component';

@Injectable()
export abstract class DetailBaseComponent extends BaseComponent {
  id: number;
  idParam: string;
  isBefore: boolean | undefined | null;
  pageStatus: number;
  pageStatusName: string;

  administrator: { id: number, name: string } | null;
  afterId: number | null | undefined;
  editStatus?: EditStatus;
  editorId?: number;
  editorName?: string;
  editCreateDt?: Date;
  editReviewNote: string | null;
  editReviewNotes: ReviewNote[] = [];
  contentCreateDt?: Date | null;

  unitCategories: { id: number, name: string, selected: boolean }[] = [];

  constructor(
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService,
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected dialog: MatDialog,
    @Inject(String) idParam = 'id') {
    super(unitService);
    this.idParam = idParam;

    this.getQueryParams();
    this.setAdministrator();
    this.setPageStatus();
  }

  getQueryParams(): void {
    this.route.queryParams.subscribe(response => {
      try {
        if (response[this.idParam] != undefined) {
          this.id = parseInt(response[this.idParam]);
        }
        if (response['isBefore'] != undefined) {
          this.isBefore = response['isBefore'].toLowerCase() === 'true';
        }
      } catch (ex) {
      }
    })
  }

  setAdministrator() {
    this.administrator = this.authService.getAdministrator();
  }

  getPageStatusName(status: number): string {
    switch (status) {
      case PageStatus.Create:
        return '新增';
      case PageStatus.Edit:
        return '編輯';
      case PageStatus.Review:
        return '審核';
      default:
        return '新增';
    }
  }

  setPageStatus(): void {
    this.route.queryParams.subscribe(response => {
      try {
        const _pageStatus = parseInt(response['status']);
        this.pageStatus = (_pageStatus == PageStatus.Edit || _pageStatus == PageStatus.Review) ? _pageStatus : PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(this.pageStatus);
      } catch {
        this.pageStatus = PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(PageStatus.Create);
      }
    })
  }

  handleFormStatus(form: FormGroup) {
    if (this.isPreventEdit()) {
      form.disable();
      return;
    }
    form.enable();
  }

  isIdInit(): boolean {
    return this.id != null && this.id != -1;
  }

  isReviewNoteEmpty(): boolean {
    return this.editReviewNote == null || this.editReviewNote.trim().length == 0;
  }

  isPreventEdit(): boolean {
    return this.editStatus == EditStatus.Review || (this.editStatus == EditStatus.Reject && this.administrator?.id != this.editorId);
  }

  setEditData(
    editorId?: number,
    editorName?: string,
    contentCreateDt?: Date | null,
    editStatus?: EditStatus,
    editReviewNotes: ReviewNote[] = [],
    afterId?: number | null | undefined
  ): void {
    this.editorId = editorId;
    this.editorName = editorName;
    this.contentCreateDt = contentCreateDt;
    this.editStatus = editStatus;
    this.editReviewNotes = editReviewNotes;
    this.afterId = afterId;
  }

  openReviewNoteDialog(): void {
    this.dialog.open(ReviewNoteDialogComponent, {
      width: '474px',
      data: new ReviewNoteDialogData(this.editorName, this.editReviewNotes, this.contentCreateDt)
    });
  }

  getCategories() {
    if (!this.isUnitInit()) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      if (response.entries != null) {
        response.entries.forEach(item => {
          this.unitCategories.push({ id: item.id, name: item.name, selected: false });
        })
      }
    });
  }

  updateCategories(categories: Category[] | null): void {
    if (categories == null) { return; }

    categories.forEach(item => {
      this.unitCategories.forEach(item2 => {
        if (item2.id == item.id) {
          item2.selected = true;
        }
      })
    })
  }

  getListSelectedIDs(list: MatSelectionList) {
    if (list?.selectedOptions?.selected == null) { return [] };

    let ids: number[] = [];
    list.selectedOptions.selected.forEach(item => {
      ids.push(item.value)
    })
    return ids;
  }
}
