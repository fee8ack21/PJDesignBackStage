import { Inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EditStatus, FormControlErrorType, PageStatus } from '../../models/enums';
import { ReviewNote } from '../../models/review-note';
import { AuthService } from '../../services/auth.service';
import { UnitService } from '../../services/unit-service';

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
          this.isBefore = response['isBefore'].toLower() === 'true';
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
}
