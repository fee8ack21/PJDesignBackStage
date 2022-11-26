import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageStatus } from 'src/app/shared/models/enums';

@Component({
  selector: 'app-administrator-detail',
  templateUrl: './administrator-detail.component.html',
  styleUrls: ['./administrator-detail.component.scss']
})
export class AdministratorDetailComponent implements OnInit {
  pageStatus: number;
  pageStatusName: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.setPageStatus();
  }

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
