import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControlErrorType } from '../models/enums';

@Injectable()

export class ValidatorService {
  readonly requiredErrorTxt = '欄位不得為空';
  readonly patternErrorTxt = '欄位格式錯誤';

  static readonly reviewErrorTxt = '請填寫備註';

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
        return this.requiredErrorTxt;
      case FormControlErrorType.Pattern:
        return this.patternErrorTxt;
      default:
        return '';
    }
  }
}
