import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UnitList } from '../../models/get-units';
import { AuthService } from '../../services/auth.service';
import { ProgressBarService } from '../../services/progress-bar.service';
import { UnitService } from '../../services/unit-service';
import { BaseComponent } from '../base/base.component';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent extends BaseComponent implements OnInit {
  administratorName = '';
  fixedUnits: UnitList[] = [];
  customUnits: UnitList[] = [];
  isShowProgressBar = false;

  constructor(
    public unitService: UnitService,
    public router: Router,
    private authSerivce: AuthService,
    private progressBarService: ProgressBarService) {
    super(unitService);
  }

  ngOnInit(): void {
    this.unitService.getBackStageUnitsByGroupId()

    this.getAdministratorName();
    this.listenProgrssBarService();
    this.listenUnitService();
  }

  getAdministratorName() {
    this.administratorName = this.authSerivce.getAdministrator()?.name ?? '';
  }

  logout(e: any) {
    if (e != undefined) { e.preventDefault(); }

    this.authSerivce.removeToken();
    this.authSerivce.removeAdministrator();
    this.unitService.clearUnits();
    this.router.navigate(['/']);
  }

  listenUnitService() {
    this.unitService.units$.subscribe(response => {
      this.fixedUnits = response.fixedUnits;
      this.customUnits = response.customUnits;
    })
  }

  listenProgrssBarService() {
    this.progressBarService.isShow$.subscribe(response => {
      this.isShowProgressBar = response;
    })
  }
}
