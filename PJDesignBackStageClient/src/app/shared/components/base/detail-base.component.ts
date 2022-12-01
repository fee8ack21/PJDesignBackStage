import { Inject, Injectable, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControlErrorType, PageStatus } from '../../models/enums';

@Injectable()
export abstract class DetailBaseComponent {
  id: number;
  idParam: string;
  pageStatus: number;
  pageStatusName: string;

  constructor(protected route: ActivatedRoute, @Inject(String) idParam = 'id') {
    this.idParam = idParam;
    this.setId();
    this.setPageStatus();
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  setId() {
    this.route.queryParams.subscribe(response => {
      try {
        if (response[this.idParam] == undefined) { return; }
        this.id = parseInt(response[this.idParam]);
      } catch (ex) {
      }
    })
  }

  setPageStatus() {
    this.route.queryParams.subscribe(response => {
      try {
        const _pageStatus = parseInt(response['status']);
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
      case PageStatus.Create:
        return '新增';
      case PageStatus.Edit:
        return '編輯';
      case PageStatus.Review:
        return '審核'
      default:
        return '新增'
    }
  }
}
