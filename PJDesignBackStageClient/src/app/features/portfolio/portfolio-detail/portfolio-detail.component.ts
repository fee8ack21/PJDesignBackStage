import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { ReviewNote } from 'src/app/shared/models/review-note';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdatePortfolioRequest } from '../feature-shared/models/create-or-update-portfolio';
import { GetPortfolioByIdResponse } from '../feature-shared/models/get-portfolio-by-id';

@Component({
  selector: 'app-portfolio-detail',
  templateUrl: './portfolio-detail.component.html',
  styleUrls: ['./portfolio-detail.component.scss']
})
export class PortfolioDetailComponent extends DetailBaseComponent implements OnInit {
  form: FormGroup;
  thumbnailUrl = '';
  thumbnailName = '';
  photos: string[] = [];

  @ViewChild('categorySelectEle') categorySelectEle: MatSelectionList;

  constructor(
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService,
    public validatorService: ValidatorService,
    private router: Router,
    protected dialog: MatDialog) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.initForm();
    this.unitService.isBackStageUnitsInit$.subscribe(response => {
      this.setUnit();
      this.getCategories();
      this.getPortfolio();
    });
  }

  initForm() {
    this.form = new FormGroup({
      id: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
      date: new FormControl(null)
    });
  }

  getPortfolio() {
    if (!this.isIdInit()) { return; }

    this.httpService.get<ResponseBase<GetPortfolioByIdResponse>>(`portfolio/getPortfolioById?id=${this.id}&isBefore=${this.isBefore}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.setEditData(
        response.entries?.editorId,
        response.entries?.editorName,
        response.entries?.createDt,
        response.entries?.editStatus,
        response.entries?.notes as ReviewNote[] ?? [],
        response.entries?.afterId
      );

      this.thumbnailUrl = response.entries?.thumbnailUrl ?? '';
      this.thumbnailName = response.entries?.thumbnailUrl != null ? response.entries.thumbnailUrl.split('/')[response.entries.thumbnailUrl.split('/').length - 1] : '';
      this.photos = response.entries?.photos ?? [];

      this.handleFormStatus(this.form);
      this.updateForm(response.entries!);
      this.updateCategories(response.entries!.categories);
    });
  }

  updateForm(data: GetPortfolioByIdResponse) {
    this.form.patchValue({
      id: this.isBefore ? data.id : null,
      title: data.title,
      isEnabled: data.isEnabled,
      date: data.date
    })
  }

  removePhoto(index: number) {
    this.photos.splice(index, 1);
  }

  onPhotoUpload(e: any, type = 'photo') {
    const file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    const formData = new FormData();
    formData.append('image', file, file.name);

    this.httpService.post<ResponseBase<string>>('upload/uploadPhoto', formData, { headers: new HttpHeaders() }).subscribe(response => {
      if (response.statusCode == StatusCode.Success) {
        switch (type) {
          case 'photo':
            this.photos.push(response.entries!);
            return;
          case 'thumbnail':
            this.thumbnailUrl = response.entries!;
            this.thumbnailName = file.name;
            return;
          default:
            return;
        }
      }
    })
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) { e.preventDefault(); }

    let isValidate = true;
    let snackBarText = '';
    if (!this.validateForm(this.form)) { isValidate = false; }

    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.editReviewNoteErrFlag = true;
      isValidate = false;
    }

    if (this.thumbnailUrl.length == 0) {
      snackBarText += '請上傳縮圖';
      isValidate = false;
    }

    if (!isValidate) {
      if (snackBarText.length > 0) {
        this.snackBarService.showSnackBar(snackBarText);
      }
      return;
    }

    let request: CreateOrUpdatePortfolioRequest = {
      ...this.form.value,
      editStatus: status,
      categoryIDs: this.getListSelectedIDs(this.categorySelectEle),
      afterId: this.isBefore ? this.afterId : this.id,
      photos: this.photos,
      thumbnailUrl: this.thumbnailUrl,
    };

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
    }

    this.httpService.post<ResponseBase<string>>('portfolio/createOrUpdatePortfolio', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.router.navigate(['/portfolio']);
    });
  }
}
