import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ResponseBase } from 'src/app/shared/models/bases';
import { PageStatus, StatusCode } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
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
  administratorForm: FormGroup;
  groups: GetGroupsResponse[] = [];

  constructor(
    protected route: ActivatedRoute,
    private router: Router,
    private httpService: HttpService,
    private snackBarService: SnackBarService,
    public validatorService: ValidatorService) {
    super(route);
  }

  ngOnInit(): void {
    this.initForm();
    this.getGroups();
    this.getAdministratorById();
  }

  getAdministratorById() {
    if (this.pageStatus == PageStatus.Create || this.id == null) {
      return;
    }
    this.httpService.get<ResponseBase<GetAdministratorByIdResponse>>(`administrator/GetAdministratorById?id=${this.id}`).subscribe(response => {
      if (response.statusCode == StatusCode.Fail) {
        this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);
        return;
      }

      this.updateForm(response.entries!);
    });
  }

  initForm() {
    this.administratorForm = new FormGroup({
      id: new FormControl(null),
      account: new FormControl(null, [Validators.required]),
      name: new FormControl(null, [Validators.required]),
      password: new FormControl(null, this.pageStatus == PageStatus.Create ? [Validators.required] : null),
      groupId: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(null, [Validators.required]),
    });
  }

  updateForm(data: GetAdministratorByIdResponse) {
    this.administratorForm.patchValue({ ...data });
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

  onSubmit() {
    if (this.administratorForm.invalid) {
      this.administratorForm.markAllAsTouched();
      return;
    }

    let request = new CreateOrUpdateAdministratorRequest();
    request = { ...this.administratorForm.value };

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
