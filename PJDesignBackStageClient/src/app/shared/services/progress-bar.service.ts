import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable()

export class ProgressBarService {
  isShow$ = new BehaviorSubject<boolean>(false);

  constructor() { }

  show() {
    this.isShow$.next(true);
  }

  hide() {
    this.isShow$.next(false);
  }
}
