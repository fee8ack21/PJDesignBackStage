import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ResponseBase } from 'src/app/shared/models/bases';
import { FormControlErrorType, StatusCode } from 'src/app/shared/models/enums';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import { AuthLoginRequest, AuthLoginResponse } from '../featured-shared/models/auth-login';

@Component({
  selector: 'app-auth-login',
  templateUrl: './auth-login.component.html',
  styleUrls: ['./auth-login.component.scss']
})
export class AuthLoginComponent implements OnInit {
  form: FormGroup;

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  constructor(
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private snackBarService: SnackBarService,
    private authService: AuthService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.checkIfLogin();
    this.initForm();
  }

  checkIfLogin() {
    if (this.authService.getToken() == null) { return; }
    this.router.navigate(['/administrator']);
  }

  initForm() {
    this.form = new FormGroup({
      account: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required])
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    let request: AuthLoginRequest = { ...this.form.value };

    this.httpService.post<ResponseBase<AuthLoginResponse>>('auth/login', request).subscribe(response => {
      if (response?.statusCode == null || response.statusCode == StatusCode.Fail || typeof response.entries == null) {
        this.snackBarService.showSnackBar(response.message ?? "登入失敗");
        return;
      }

      this.authService.setAdministrator({ id: response.entries!.id, name: response.entries!.name, groupId: response.entries!.groupId });
      this.authService.setToken(response.entries!.token);
      this.snackBarService.showSnackBar(response.message ?? "登入成功");
      this.router.navigate(['/administrator']);
    })
  }
}
