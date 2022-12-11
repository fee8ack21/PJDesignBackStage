import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectionList } from '@angular/material/list';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { EditStatus, StatusCode } from 'src/app/shared/models/enums';
import { GetCategoriesByUnitId } from 'src/app/shared/models/get-categories-by-unit-id';
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
  portfolioForm: FormGroup;
  unitCategories: { id: number, name: string, selected: boolean }[] = [];

  photos: string[] = [];

  @ViewChild('categorySelectEle') categorySelectEle: MatSelectionList;

  constructor(
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService,
    public validatorService: ValidatorService,
    private router: Router,
    protected dialog: MatDialog) {
    super(route, authService, unitService, dialog);
  }

  ngOnInit(): void {
    this.initForm();
    this.unitService.isBackStageUnitsInit.subscribe(response => {
      this.setUnit();
      this.getCategories();
      this.getPortfolio();
    });
  }

  initForm() {
    this.portfolioForm = new FormGroup({
      id: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
      date: new FormControl(null)
    });
  }

  getCategories() {
    if (this.unit.id == -1) { return; }

    this.httpService.get<ResponseBase<GetCategoriesByUnitId[]>>(`category/getCategoriesByUnitId?id=${this.unit.id}`).subscribe(response => {
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

  getPortfolio() {
    if (this.id == null || this.id == -1) { return; }

    this.httpService.get<ResponseBase<GetPortfolioByIdResponse>>(`portfolio/getPortfolioById?id=${this.id}&isBefore=${this.isBefore}`).subscribe(response => {
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
      this.photos = response.entries?.photos ?? [];

      this.handleFormStatus(this.portfolioForm);
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

  getSelectedCategoryIDs() {
    let categoryIDs: number[] = [];
    if (this.categorySelectEle?.selectedOptions?.selected == null) { return categoryIDs };

    this.categorySelectEle.selectedOptions.selected.forEach(item => {
      categoryIDs.push(item.value)
    })
    return categoryIDs;
  }

  updateForm(data: GetPortfolioByIdResponse) {
    this.portfolioForm.patchValue({
      // id 為before id
      id: this.isBefore ? data.id : null,
      title: data.title,
      isEnabled: data.isEnabled,
      date: data.date
    })
  }

  removePhoto(index: number) {
    this.photos.splice(index, 1);
  }

  onPhotoUpload(e: any) {
    const file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    this.httpService.postPhoto<ResponseBase<string>>('upload/uploadPhoto', file).subscribe(response => {
      if (response.statusCode == StatusCode.Success) {
        this.photos.push(response.entries!);
        return;
      }
    })
  }

  onSubmit(e: any, status: EditStatus = EditStatus.Review) {
    if (e !== undefined) {
      e.preventDefault();
    }

    if (status == EditStatus.Reject && this.isReviewNoteEmpty()) {
      this.snackBarService.showSnackBar('請填寫備註');
      return;
    }

    if (this.portfolioForm.invalid) {
      this.portfolioForm.markAllAsTouched();
      return;
    }

    let request: CreateOrUpdatePortfolioRequest = {
      ...this.portfolioForm.value,
      editStatus: status,
      categoryIDs: this.getSelectedCategoryIDs(),
      afterId: this.isBefore ? this.afterId : this.id,
      photos: this.photos
    };

    if (status == EditStatus.Reject) {
      let temp = new ReviewNote();
      temp.note = this.editReviewNote!;
      temp.name = this.administrator!.name
      request.note = temp;
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
