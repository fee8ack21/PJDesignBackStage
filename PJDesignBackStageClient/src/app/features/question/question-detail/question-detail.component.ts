import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { ValidatorService } from 'src/app/shared/services/validator.service';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent extends DetailBaseComponent implements OnInit {
  questionForm: FormGroup;
  public Editor = ClassicEditor;

  constructor(
    protected route: ActivatedRoute,
    public validatorService: ValidatorService) {
    super(route);
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.questionForm = new FormGroup({
      id: new FormControl(null, [Validators.required]),
      title: new FormControl(null, [Validators.required]),
      isEnabled: new FormControl(true, [Validators.required]),
      content: new FormControl(null, [Validators.required]),
    });
  }

  onSubmit() {
    return;
  }
}
