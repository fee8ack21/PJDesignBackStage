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
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateType2ContentRequest } from '../feature-shared/models/create-or-update-type2-content';
import { GetType2ContentByIdResponse } from '../feature-shared/models/get-type2-content-by-id';

@Component({
  selector: 'app-type2-detail',
  templateUrl: './type2-detail.component.html',
  styleUrls: ['./type2-detail.component.scss']
})
export class Type2DetailComponent extends DetailBaseComponent implements OnInit {
  form: FormGroup;
  thumbnailUrl = '';
  thumbnailName = '';
  imageUrl = '';
  imageName = '';

  @ViewChild('categorySelectEle') categorySelectEle: MatSelectionList;

  constructor(
    protected route: ActivatedRoute,
    protected snackBarService: SnackBarService,
    protected httpService: HttpService,
    public validatorService: ValidatorService,
    protected unitService: UnitService,
    private spinnerService: SpinnerService,
    private router: Router,
    protected dialog: MatDialog,
    protected authService: AuthService) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(async response => {
      this.setUnit();

      this.initForm();
      this.fetchPageData();
    });
  }

  async fetchPageData() {
    this.spinnerService.isShow$.next(true);
    this.handleCategoriesResponse(await this.getCategoriesPromise());
    this.handleType2Response(await this.getType2ContentPromise());
    this.spinnerService.isShow$.next(false);
  }

  getType2ContentPromise() {
    if (!this.isIdInit() || !this.isUnitInit()) { return; }
    return this.httpService.get<ResponseBase<GetType2ContentByIdResponse>>(`type2/getType2ContentById?unitId=${this.unit.id}&id=${this.id}&isBefore=${this.isBefore}`).toPromise();
  }
  handleType2Response(response: ResponseBase<GetType2ContentByIdResponse> | undefined) {
    if (response == undefined) { return; }

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
    this.imageUrl = response.entries?.imageUrl ?? '';
    this.imageName = response.entries?.imageUrl != null ? response.entries.imageUrl.split('/')[response.entries.imageUrl.split('/').length - 1] : '';

    this.handleFormStatus(this.form);
    this.updateForm(response.entries!);
    this.updateCategories(response.entries!.categories);
  }

  initForm() {
    this.form = new FormGroup({
      id: new FormControl(null),
      unitId: new FormControl(this.unit.id),
      title: new FormControl(null, [Validators.required]),
      description: new FormControl(null),
      isEnabled: new FormControl(true, [Validators.required]),
      isFixed: new FormControl(false, [Validators.required]),
      content: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: any) {
    this.form.patchValue({
      id: this.isBefore ? data.id : null,
      unitId: this.unit.id,
      title: data.title,
      isEnabled: data.isEnabled,
      content: data.content,
      isFixed: data.isFixed,
      description: data.description
    })
  }

  onPhotoUpload(e: any, type: string) {
    const file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];

    if (file == undefined) { return; }

    const formData = new FormData();
    formData.append('image', file, file.name);

    this.httpService.post<ResponseBase<string>>('upload/uploadPhoto', formData, { headers: new HttpHeaders() }).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      if (type == 'thumbnail') {
        this.thumbnailUrl = response.entries!;
        this.thumbnailName = file.name;
      } else if (type == 'image') {
        this.imageUrl = response.entries!;
        this.imageName = file.name;
      }
    })
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) {
      e.preventDefault();
    }

    let isValidate = true;
    let snackBarText = '';
    if (!this.validateForm(this.form)) { isValidate = false; }

    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.editReviewNoteErrFlag = true;
      isValidate = false;
    }

    if (this.thumbnailUrl.length == 0) {
      isValidate = false;
      snackBarText += '請上傳縮圖';
    }

    if (this.imageUrl.length == 0) {
      isValidate = false;
      if (snackBarText.length > 0) {
        snackBarText += '及Banner圖';
      } else {
        snackBarText += '請上傳Banner圖';
      }
    }

    if (!isValidate) {
      if (snackBarText.length > 0) {
        this.snackBarService.showSnackBar(snackBarText);
      }
      return;
    }

    let request: CreateOrUpdateType2ContentRequest = {
      ...this.form.value,
      editStatus: status,
      categoryIDs: this.getListSelectedIDs(this.categorySelectEle),
      afterId: this.isBefore ? this.afterId : this.id,
      thumbnailUrl: this.thumbnailUrl,
      imageUrl: this.imageUrl,
    };

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
    }

    this.httpService.post<ResponseBase<string>>('type2/createOrUpdateType2Content', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.router.navigate(['/type2'], { queryParams: { uid: this.unit.id } });
    });
  }
}

