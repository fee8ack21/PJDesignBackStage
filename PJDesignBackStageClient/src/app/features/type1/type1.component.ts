import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
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
import { CreateOrUpdateType1ContentRequest } from './feature-shared/models/create-or-update-type1-content';
import { GetType1ContentResponse } from './feature-shared/models/get-type1-content';

@Component({
  selector: 'app-type1',
  templateUrl: './type1.component.html',
  styleUrls: ['./type1.component.scss']
})
export class Type1Component extends DetailBaseComponent implements OnInit {
  form: FormGroup;

  constructor(
    protected httpService: HttpService,
    public validatorService: ValidatorService,
    protected snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected unitService: UnitService,
    protected authService: AuthService,
    private router: Router,
    protected dialog: MatDialog) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

    this.initForm();
    this.unitService.isBackStageUnitsInit$.subscribe(async response => {
      this.setUnit();
      this.getType1Content();
    });
  }

  initForm() {
    this.form = new FormGroup({
      unitId: new FormControl(null),
      content: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetType1ContentResponse) {
    this.form.patchValue({
      unitId: this.unit.id,
      content: data.content
    })
  }

  getType1Content() {
    if (!this.isUnitInit()) { return; }

    this.httpService.get<ResponseBase<GetType1ContentResponse>>(`type1/getType1ContentByUnitId?id=${this.unit.id}`).subscribe(response => {
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

      this.handleFormStatus(this.form);
      this.updateForm(response.entries!);
    });
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) {
      e.preventDefault();
    }

    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.snackBarService.showSnackBar(SnackBarService.ReviewErrorText);
      return;
    }

    if (!this.validateForm(this.form)) { return; }

    let request: CreateOrUpdateType1ContentRequest = {
      ...this.form.value,
      editStatus: status,
    };

    if (status == this.EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
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
