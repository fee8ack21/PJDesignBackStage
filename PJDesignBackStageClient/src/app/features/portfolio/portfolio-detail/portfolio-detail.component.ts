import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DetailBaseComponent } from 'src/app/shared/components/base/detail-base.component';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UnitService } from 'src/app/shared/services/unit-service';

@Component({
  selector: 'app-portfolio-detail',
  templateUrl: './portfolio-detail.component.html',
  styleUrls: ['./portfolio-detail.component.scss']
})
export class PortfolioDetailComponent extends DetailBaseComponent implements OnInit {
  constructor(
    protected route: ActivatedRoute,
    protected authService: AuthService,
    protected unitService: UnitService) {
    super(route, authService, unitService);
  }

  ngOnInit(): void {
    this.setPageStatus();
  }
}
