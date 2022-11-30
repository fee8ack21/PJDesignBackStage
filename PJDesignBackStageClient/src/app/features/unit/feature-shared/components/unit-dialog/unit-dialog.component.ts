import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UnitDialogData } from '../../models/unit-dialog-data';

@Component({
  selector: 'app-unit-dialog',
  templateUrl: './unit-dialog.component.html',
  styleUrls: ['./unit-dialog.component.scss']
})
export class UnitDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<UnitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UnitDialogData) { }

  ngOnInit(): void {
  }

  onCancelBtnClick(): void {
    this.dialogRef.close();
  }
}
