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
import { CreateOrUpdateQuestionRequest } from '../feature-shared/models/create-or-update-question';
import { GetQuestionByIdResponse } from '../feature-shared/models/get-question-by-id';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent extends DetailBaseComponent implements OnInit {
  form: FormGroup;

  @ViewChild('categorySelectEle') categorySelectEle: MatSelectionList;

  constructor(
    protected route: ActivatedRoute,
    protected snackBarService: SnackBarService,
    protected httpService: HttpService,
    public validatorService: ValidatorService,
    protected unitService: UnitService,
    private router: Router,
    protected dialog: MatDialog,
    protected authService: AuthService) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.initForm();
    this.unitService.isBackStageUnitsInit$.subscribe(async response => {
      this.setUnit();
      this.getCategories();
      this.getQuestion();
    });
  }

  getQuestion() {
    if (!this.isIdInit()) { return; }

    this.httpService.get<ResponseBase<GetQuestionByIdResponse>>(`question/getQuestionById?id=${this.id}&isBefore=${this.isBefore}`).subscribe(response => {
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
      this.updateCategories(response.entries!.categories);
    });
  }

  initForm() {
    this.form = new FormGroup({
      id: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
      content: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetQuestionByIdResponse) {
    this.form.patchValue({
      id: this.isBefore ? data.id : null,
      title: data.title,
      isEnabled: data.isEnabled,
      content: data.content
    })
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) { e.preventDefault(); }

    let isValidate = true;
    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      isValidate = false;
      this.editReviewNoteErrFlag = true;
    }

    if (!this.validateForm(this.form)) { isValidate = false; }

    if (!isValidate) { return; }

    let request: CreateOrUpdateQuestionRequest = {
      ...this.form.value,
      editStatus: status,
      categoryIDs: this.getListSelectedIDs(this.categorySelectEle),
      afterId: this.isBefore ? this.afterId : this.id
    };

    if (status == EditStatus.Reject) {
      request.note = new ReviewNote(this.administrator!.name, this.editReviewNote!);
    }

    this.httpService.post<ResponseBase<string>>('question/createOrUpdateQuestion', request).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.router.navigate(['/question']);
    });
  }
}
