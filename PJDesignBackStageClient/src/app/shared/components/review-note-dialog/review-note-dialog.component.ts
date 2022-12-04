import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReviewNoteDialogData } from '../../models/review-note-dialog-data';

@Component({
  selector: 'app-review-note-dialog',
  templateUrl: './review-note-dialog.component.html',
  styleUrls: ['./review-note-dialog.component.scss']
})
export class ReviewNoteDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: ReviewNoteDialogData) { }

  ngOnInit(): void {
  }
}
