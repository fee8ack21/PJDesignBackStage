import { Injectable, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControlErrorType, PageStatus } from '../../models/enums';

@Injectable()
export abstract class DetailBaseComponent {
  pageStatus: number;
  pageStatusName: string;

  constructor(protected route: ActivatedRoute) { }

  setPageStatus() {
    this.route.queryParams.subscribe(res => {
      try {
        const _pageStatus = parseInt(res['status']);
        this.pageStatus = (_pageStatus == PageStatus.Edit || _pageStatus == PageStatus.Review) ? _pageStatus : PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(this.pageStatus);
      } catch (ex) {
        this.pageStatus = PageStatus.Create;
        this.pageStatusName = this.getPageStatusName(PageStatus.Create);
      }
    })
  }

  getPageStatusName(status: number) {
    switch (status) {
      case 0:
        return '新增';
      case 1:
        return '編輯';
      case 2:
        return '審核'
      default:
        return '新增'
    }
  }
}
