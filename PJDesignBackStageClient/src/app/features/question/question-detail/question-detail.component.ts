import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ReviewNoteDialogComponent } from 'src/app/shared/components/review-note-dialog/review-note-dialog.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { defaultEditorConfig } from 'src/app/shared/models/editor-config';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetCategoriesByUnitId } from 'src/app/shared/models/get-categories-by-unit-id';
import { ReviewNoteDialogData } from 'src/app/shared/models/review-note-dialog-data';
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
  questionForm: FormGroup;
  editorConfig = defaultEditorConfig;
  unitCategories: { id: number, name: string, selected: boolean }[] = [];

  @ViewChild('categorySelectEle') categorySelectEle: MatSelectionList;

  constructor(
    protected route: ActivatedRoute,
    private snackBarService: SnackBarService,
    private httpService: HttpService,
    public validatorService: ValidatorService,
    protected unitService: UnitService,
    private router: Router,
    protected dialog: MatDialog,
    protected authService: AuthService) {
    super(route, authService, unitService, dialog);
  }

  ngOnInit(): void {
    this.initForm();
    this.listenUnitService();
  }

  listenUnitService() {
    this.unitService.isBackStageUnitsInit.subscribe(async response => {
      this.setUnitId();
      this.getCategories();
      this.getQuestion();
    });
  }

  getQuestion() {
    this.httpService.get<ResponseBase<GetQuestionByIdResponse>>(`question/getQuestionById?id=${this.id}&isBefore=${this.isBefore}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.editStatus = response.entries?.editStatus;
      this.editorId = response.entries?.editorId;

      this.handleFormStatus(this.questionForm);
      this.updateForm(response.entries!);
      response.entries!.categories?.forEach(item => {
        this.unitCategories.forEach(item2 => {
          if (item2.id == item.id) {
            item2.selected = true;
          }
        })
      })
    });
  }

  initForm() {
    this.questionForm = new FormGroup({
      id: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
      content: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetQuestionByIdResponse) {
    this.questionForm.patchValue({
      id: data.id,
      title: data.title,
      isEnabled: data.isEnabled,
      content: data.content
    })
  }

  getCategories() {
    if (this.unitId == -1) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      if (response.entries != null) {
        response.entries.forEach(item => {
          let temp = { id: item.id, name: item.name, selected: false }
          this.unitCategories.push(temp);
        })
      }
    });
  }

  getSelectedCategoryIDs() {
    let categoryIDs: number[] = [];
    if (this.categorySelectEle?.selectedOptions?.selected == null) { return categoryIDs };

    this.categorySelectEle.selectedOptions.selected.forEach(item => {
      categoryIDs.push(item.value)
    })
    return categoryIDs;
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) {
      e.preventDefault();
    }

    if (this.questionForm.invalid) {
      this.questionForm.markAllAsTouched();
    }

    let request: CreateOrUpdateQuestionRequest = { ...this.questionForm.value, editStatus: status, categoryIDs: this.getSelectedCategoryIDs() };
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
