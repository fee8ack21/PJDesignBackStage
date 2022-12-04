import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetBackStageUnitsByGroupIdResponse } from '../../models/get-back-stage-units-by-group-id';
import { AuthService } from '../../services/auth.service';
import { HttpService } from '../../services/http.service';
import { ProgressBarService } from '../../services/progress-bar.service';
import { UnitService } from '../../services/unit-service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  administratorName = ''

  fixedUnits: GetBackStageUnitsByGroupIdResponse[] = [];
  customUnits: GetBackStageUnitsByGroupIdResponse[] = [];

  isShowProgressBar = false;

  constructor(
    private httpService: HttpService,
    public unitService: UnitService,
    public router: Router,
    private authSerivce: AuthService,
    private progressBarService: ProgressBarService) { }

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
    this.router.navigate(['/']);
  }

  listenProgrssBarService() {
    this.progressBarService.isShow.subscribe(response => {
      this.isShowProgressBar = response;
    })
  }
}
