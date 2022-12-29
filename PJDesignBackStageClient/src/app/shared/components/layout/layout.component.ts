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
    this.getAdministratorName();
    this.getUnits();
    this.listenProgrssBarService();
  }

  getAdministratorName() {
    this.administratorName = this.authSerivce.getAdministrator()?.name ?? '';
  }

  async getUnits() {
    let temp = await this.unitService.getBackStageUnitsByGroupId();
    this.fixedUnits = temp.fixedUnits;
    this.customUnits = temp.customUnits;
  }

  logout(e: any) {
    if (e != undefined) { e.preventDefault(); }

    this.authSerivce.removeToken();
    this.authSerivce.removeAdministrator();
    this.unitService.clearUnits();
    this.router.navigate(['/']);
  }

  listenProgrssBarService() {
    this.progressBarService.isShow.subscribe(response => {
      this.isShowProgressBar = response;
    })
  }
}
