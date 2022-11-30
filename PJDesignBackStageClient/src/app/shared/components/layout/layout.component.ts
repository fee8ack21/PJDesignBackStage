import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ResponseBase } from '../../models/bases';
import { GetBackStageUnitsByGroupIdResponse } from '../../models/get-back-stage-units-by-group-id';
import { AuthService } from '../../services/auth.service';
import { HttpService } from '../../services/http.service';
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

  constructor(
    private httpService: HttpService,
    public unitService: UnitService,
    public router: Router,
    private authSerivce: AuthService) { }

  ngOnInit(): void {
    this.getAdministratorName();
    this.getUnits();
  }

  getAdministratorName() {
    this.administratorName = this.authSerivce.getAdministratorName() ?? '';
  }

  async getUnits() {
    let temp = await this.unitService.getBackStageUnitsByGroupId();
    this.fixedUnits = temp[0];
    this.customUnits = temp[1];
  }

  logout(e: any) {
    if (e != undefined) {
      e.preventDefault();
    }

    this.authSerivce.removeToken();
    this.router.navigate(['/']);
  }
}
