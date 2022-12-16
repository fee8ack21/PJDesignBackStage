import { HttpEvent } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AngularEditorConfig, UploadResponse } from '@kolkov/angular-editor';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { defaultEditorConfig } from 'src/app/shared/models/editor-config';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetSettingByUnitIdResponse } from 'src/app/shared/models/get-setting-by-unit-id';
import { ReviewNote } from 'src/app/shared/models/review-note';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateType1ContentRequest } from './feature-shared/models/create-or-update-type1-content';
import { GetType1ContentResponse } from './feature-shared/models/get-type1-content';

@Component({
  selector: 'app-type1',
  templateUrl: './type1.component.html',
  styleUrls: ['./type1.component.scss']
})
export class Type1Component extends DetailBaseComponent implements OnInit {
  type1Form: FormGroup;
  editorConfig: AngularEditorConfig = {
    ...defaultEditorConfig,
    upload: (file: File): Observable<HttpEvent<UploadResponse>> => {
      return this.httpService
        .postPhoto<ResponseBase<string>>('upload/uploadPhoto', file)
        .pipe(
          map((x: any) => {
            x.body = { imageUrl: x.entries };
            return x;
          })
        );
    },
  };

  constructor(
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    private router: Router,
    protected dialog: MatDialog) {
    super(route, authService, unitService, dialog);
  }

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

    this.initForm();
    this.unitService.isBackStageUnitsInit.subscribe(async response => {
      this.setUnit();
      this.getSettingByUnitId();
      this.getType1Content();
    });
  }

  initForm() {
    this.type1Form = new FormGroup({
      unitId: new FormControl(null),
      content: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetType1ContentResponse) {
    this.type1Form.patchValue({
      unitId: this.unit.id,
      content: data.content
    })
  }

  getType1Content() {
    if (this.unit.id <= 0) { return; }

    this.httpService.get<ResponseBase<GetType1ContentResponse>>(`type1/getType1ContentByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.editStatus = response.entries?.editStatus;
      this.contentCreateDt = response.entries?.createDt;
      this.editorId = response.entries?.editorId;
      this.editReviewNotes = response.entries?.notes as ReviewNote[] ?? [];
      this.editorName = response.entries?.editorName;
      this.afterId = response.entries?.afterId;

      this.handleFormStatus(this.type1Form);
      this.updateForm(response.entries!);
    });
  }

  getSettingByUnitId() {
    if (this.unit.id == null || this.unit.id == -1) { return }

    this.httpService.get<ResponseBase<GetSettingByUnitIdResponse>>(`unit/getSettingByUnitId?id=${this.unit.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.editorId = response.entries?.editorId;
      this.editorName = response.entries?.editorName;
      this.contentCreateDt = response.entries?.createDt;
      this.editStatus = response.entries?.editStatus;
      this.editReviewNotes = response.entries?.notes as ReviewNote[] ?? [];
    });
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) {
      e.preventDefault();
    }

    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.snackBarService.showSnackBar('請填寫備註');
      return;
    }

    if (this.type1Form.invalid) {
      this.type1Form.markAllAsTouched();
      return;
    }

    let request: CreateOrUpdateType1ContentRequest = {
      ...this.type1Form.value,
      editStatus: status,
    };

    if (status == this.EditStatus.Reject) {
      let temp = new ReviewNote();
      temp.note = this.editReviewNote!;
      temp.name = this.administrator!.name
      request.note = temp;
    }

    this.httpService.post<ResponseBase<string>>('type1/createOrUpdateType1Content', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.getType1Content();
      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
    });
  }
}
