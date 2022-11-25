import { Injectable, OnInit } from '@angular/core';
import { FormControlErrorType } from '../../models/enums';

@Injectable()
export abstract class BaseComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
}
