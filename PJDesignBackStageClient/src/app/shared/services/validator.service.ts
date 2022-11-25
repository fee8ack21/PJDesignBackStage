import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControlErrorType } from '../models/enums';

@Injectable()

export class ValidatorService {
  constructor() { }

  isFormControlInvalid(form: FormGroup, controlName: string): boolean {
    if (form.get(controlName)?.errors && (form.get(controlName)?.touched || form.get(controlName)?.dirty)) {
      return true;
    }

    return false;
  }

  getFormControlErrorText(type: number): string {
    switch (type) {
      case FormControlErrorType.Required:
        return '欄位不得為空'
      default:
        return ''
    }
  }
}
