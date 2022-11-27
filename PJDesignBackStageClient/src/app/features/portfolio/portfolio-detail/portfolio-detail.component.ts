import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';

@Component({
  selector: 'app-portfolio-detail',
  templateUrl: './portfolio-detail.component.html',
  styleUrls: ['./portfolio-detail.component.scss']
})
export class PortfolioDetailComponent extends DetailBaseComponent implements OnInit {
  constructor(protected route: ActivatedRoute) {
    super(route);
  }

  ngOnInit(): void {
    this.setPageStatus();
  }
}
