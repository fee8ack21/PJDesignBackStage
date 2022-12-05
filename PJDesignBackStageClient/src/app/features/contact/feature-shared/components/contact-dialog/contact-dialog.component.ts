import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ContactDialogData } from '../../models/contact-dialog-data';

@Component({
  selector: 'app-contact-dialog',
  templateUrl: './contact-dialog.component.html',
  styleUrls: ['./contact-dialog.component.scss']
})
export class ContactDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: ContactDialogData) { }

  ngOnInit(): void {
  }

}
