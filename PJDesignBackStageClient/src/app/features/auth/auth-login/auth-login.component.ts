import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/base/base.component';
import { FormControlErrorType } from 'src/app/shared/models/enums';
import { HttpService } from 'src/app/shared/services/http.service';
import { ValidatorService } from 'src/app/shared/services/validator.service';

@Component({
  selector: 'app-auth-login',
  templateUrl: './auth-login.component.html',
  styleUrls: ['./auth-login.component.scss']
})
export class AuthLoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private httpService: HttpService,
    public validatorService: ValidatorService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.loginForm = new FormGroup({
      account: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required])
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) { return; }

    this.router.navigate(['/administrator']);
    // this.httpService.post<ResponseBase<string>>('').subscribe();
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }
}
