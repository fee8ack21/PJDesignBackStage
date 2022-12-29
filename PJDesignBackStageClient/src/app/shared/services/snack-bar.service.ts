import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable()

export class SnackBarService {
  static readonly RequestFailedText = "請求失敗";
  static readonly RequestSuccessText = "請求成功";
  static readonly ReviewErrorText = '請填寫備註';

  constructor(private snackBar: MatSnackBar) { }

  showSnackBar(text: string) {
    this.snackBar.open(text, '關閉', {
      duration: 2000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
