import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable()

export class SnackBarService {
  static RequestFailedText = "請求失敗";
  static RequestSuccessText = "請求成功";

  constructor(private snackBar: MatSnackBar) { }

  showSnackBar(text: string) {
    this.snackBar.open(text, '關閉', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
