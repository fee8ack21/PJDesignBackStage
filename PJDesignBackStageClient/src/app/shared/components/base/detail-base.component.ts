import { Inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { EditStatus, FormControlErrorType, PageStatus } from '../../models/enums';
import { ReviewNote } from '../../models/review-note';
import { ReviewNoteDialogData } from '../../models/review-note-dialog-data';
import { AuthService } from '../../services/auth.service';
import { UnitService } from '../../services/unit-service';
import { ReviewNoteDialogComponent } from '../review-note-dialog/review-note-dialog.component';

@Injectable()
export abstract class DetailBaseComponent {
  id: number;
  idParam: string;
  isBefore: boolean | undefined | null;
  pageStatus: number;
  pageStatusName: string;

  unitId: number;
  administrator: { id: number, name: string } | null;
  editStatus?: EditStatus;
  editorId?: number;
  editorName?: string;
  editCreateDt: Date;
  editReviewNote: string | null;
  editReviewNotes: ReviewNote[] = [];
  contentCreateDt?: Date | null;

  constructor(
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService,
    protected dialog: MatDialog,
    @Inject(String) idParam = 'id') {
    this.idParam = idParam;

    this.getQueryParams();
    this.setAdministrator();
    this.setPageStatus();
  }

  public get EditStatus(): typeof EditStatus {
    return EditStatus;
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  getQueryParams(): void {
    this.route.queryParams.subscribe(response => {
      try {
        if (response[this.idParam] != undefined) {
          this.id = parseInt(response[this.idParam]);
        }
        if (response['isBefore'] != undefined) {
          console.log(response['isBefore'])
          this.isBefore = response['isBefore'].toLowerCase() === 'true';
          console.log(this.isBefore)
        }
      } catch (ex) {
      }
    })
  }

  setUnitId(): void {
    this.unitId = this.unitService.getCurrentUnit();
  }

  setAdministrator() {
    this.administrator = this.authService.getAdministrator();
  }

  setPageStatus(): void {
    this.route.queryParams.subscribe(response => {
      try {
        const _pageStatus = parseInt(response['status']);
        this.pageStatus = (_pageStatus == PageStatus.Edit || _pageStatus == PageStatus.Review) ? _pageStatus : PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(this.pageStatus);
      } catch (ex) {
        this.pageStatus = PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(PageStatus.Create);
      }
    })
  }

  getPageStatusName(status: number): string {
    switch (status) {
      case PageStatus.Create:
        return '新增';
      case PageStatus.Edit:
        return '編輯';
      case PageStatus.Review:
        return '審核'
      default:
        return '新增'
    }
  }

  handleFormStatus(form: FormGroup) {
    if (this.isPreventEdit()) {
      form.disable();
      return;
    }
    form.enable();
  }

  isReviewNoteEmpty(): boolean {
    return this.editReviewNote == null || this.editReviewNote.trim().length == 0;
  }

  isPreventEdit(): boolean {
    return this.editStatus == EditStatus.Review || (this.editStatus == EditStatus.Reject && this.administrator?.id != this.editorId);
  }

  openReviewNoteDialog() {
    let data = new ReviewNoteDialogData();
    data.editorName = this.editorName;
    data.notes = this.editReviewNotes;
    data.createDt = this.contentCreateDt;

    this.dialog.open(ReviewNoteDialogComponent, {
      width: '474px',
      data: data
    });
  }
}
