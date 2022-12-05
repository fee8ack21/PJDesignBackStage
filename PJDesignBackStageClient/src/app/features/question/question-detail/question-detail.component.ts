import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { defaultEditorConfig } from 'src/app/shared/models/editor-config';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetCategoriesByUnitId } from 'src/app/shared/models/get-categories-by-unit-id';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateQuestionRequest } from '../feature-shared/models/create-or-update-question';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent extends DetailBaseComponent implements OnInit {
  questionForm: FormGroup;
  editorConfig = defaultEditorConfig;
  unitId: number;
  unitCategories: GetCategoriesByUnitId[] = [];

  @ViewChild('categories') categoriesEle: MatSelectionList;

  constructor(
    protected route: ActivatedRoute,
    private snackBarService: SnackBarService,
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private unitService: UnitService,
    private router: Router) {
    super(route);
  }

  ngOnInit(): void {
    this.initForm();
    this.listenUnitService();
  }

  listenUnitService() {
    this.unitService.isBackStageUnitsInit.subscribe(response => {
      this.setUnitId();
      this.getCategories();
    });
  }

  setUnitId() {
    this.unitId = this.unitService.getCurrentUnit();
  }

  getCategories() {
    if (this.unitId == -1) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unitId}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.unitCategories = response.entries ?? [];
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

  getSelectedCategoryIDs() {
    let categoryIDs: number[] = [];
    if (this.categoriesEle?.selectedOptions?.selected == null) { return categoryIDs };

    this.categoriesEle.selectedOptions.selected.forEach(item => {
      categoryIDs.push(item.value)
    })
    return categoryIDs;
  }

  onSubmit() {
    if (this.questionForm.invalid) {
      this.questionForm.markAllAsTouched();
    }

    let status = EditStatus.Review;
    if (this.questionForm.get('id')?.value != null) {

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
