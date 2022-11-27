import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable()

export class SnackBarService {
  constructor(private snackBar: MatSnackBar) { }

  showSnackBar(text: string) {
    this.snackBar.open(text, '關閉', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
