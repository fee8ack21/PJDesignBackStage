import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { PageStatus, StatusCode } from 'src/app/shared/models/enums';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { UnitService } from 'src/app/shared/services/unit-service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { CreateOrUpdateAdministratorRequest } from '../feature-shared/models/create-or-update-administrator';
import { GetAdministratorByIdResponse } from '../feature-shared/models/get-administrator-by-id';
import { GetGroupsResponse } from '../feature-shared/models/get-groups';

@Component({
  selector: 'app-administrator-detail',
  templateUrl: './administrator-detail.component.html',
  styleUrls: ['./administrator-detail.component.scss']
})
export class AdministratorDetailComponent extends DetailBaseComponent implements OnInit {
  form: FormGroup;
  groups: GetGroupsResponse[] = [];

  constructor(
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService,
    protected dialog: MatDialog,
    private router: Router,
    protected httpService: HttpService,
    protected snackBarService: SnackBarService,
    public validatorService: ValidatorService) {
    super(route, authService, unitService, httpService, snackBarService, dialog);
  }

  ngOnInit(): void {
    this.unitService.isBackStageUnitsInit$.subscribe(() => { this.setUnit(); });
    this.initForm();
    this.getGroups();
    this.getAdministratorById();
  }

  getGroups() {
    this.httpService.get<ResponseBase<GetGroupsResponse[]>>('administrator/getGroups').subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.groups = response.entries!;
    });
  }

  getAdministratorById() {
    if (this.pageStatus == PageStatus.Create || !this.isIdInit()) { return; }

    this.httpService.get<ResponseBase<GetAdministratorByIdResponse>>(`administrator/GetAdministratorById?id=${this.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.updateForm(response.entries!);
    });
  }

  initForm() {
    this.form = new FormGroup({
      id: new FormControl(null),
      account: new FormControl(null, [Validators.required]),
      name: new FormControl(null, [Validators.required]),
      password: new FormControl(null, this.pageStatus == PageStatus.Create ? [Validators.required] : null),
      groupId: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetAdministratorByIdResponse) {
    this.form.patchValue({ ...data });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    let request: CreateOrUpdateAdministratorRequest = { ...this.form.value };

    this.httpService.post<ResponseBase<null>>('administrator/CreateOrUpdateAdministrator', request).subscribe(response => {
      if (response?.statusCode == null || response.statusCode == StatusCode.Fail || typeof response.entries == null) {
        this.snackBarService.showSnackBar(response.message ?? SnackBarService.RequestFailedText);
        return;
      }

      this.snackBarService.showSnackBar(SnackBarService.RequestSuccessText);
      this.router.navigate(['/administrator']);
    })
  }
}
