import { Injectable, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControlErrorType, PageStatus } from '../../models/enums';

@Injectable()
export abstract class DetailBaseComponent {
  pageStatus: number;
  pageStatusName: string;

  constructor(protected route: ActivatedRoute) {
    this.setPageStatus();
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  setPageStatus() {
    this.route.queryParams.subscribe(response => {
      try {
        const _pageStatus = parseInt(response['status']);
        this.pageStatus = (_pageStatus == PageStatus.編輯 || _pageStatus == PageStatus.審核) ? _pageStatus : PageStatus.新增;
        this.pageStatusName = this.getPageStatusName(this.pageStatus);
      } catch (ex) {
        this.pageStatus = PageStatus.新增;
        this.pageStatusName = this.getPageStatusName(PageStatus.新增);
      }
    })
  }

  getPageStatusName(status: number) {
    switch (status) {
      case PageStatus.新增:
        return '新增';
      case PageStatus.編輯:
        return '編輯';
      case PageStatus.審核:
        return '審核'
      default:
        return '新增'
    }
  }
}
