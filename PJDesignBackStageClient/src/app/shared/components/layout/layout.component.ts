import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UnitList } from '../../models/get-units';
import { AuthService } from '../../services/auth.service';
import { SpinnerService } from '../../services/spinner.service';
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
  isShowSpinner = false;

  constructor(
    public unitService: UnitService,
    public router: Router,
    private authSerivce: AuthService,
    private spinnerService: SpinnerService) {
    super(unitService);
  }

  ngOnInit(): void {
    this.unitService.getBackStageUnitsByGroupId()

    this.getAdministratorName();
    this.listenSpinnerService();
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

  listenSpinnerService() {
    this.spinnerService.isShow$.subscribe(response => {
      this.isShowSpinner = response;
    })
  }
}
