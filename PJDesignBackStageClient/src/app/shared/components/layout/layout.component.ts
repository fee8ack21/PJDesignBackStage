import { Component, OnInit } from '@angular/core';
import { ResponseBase } from '../../models/bases';
import { GetUnitsResponse } from '../../models/get-units';
import { HttpService } from '../../services/http.service';
import { UnitService } from '../../services/unit-service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  userName = '測試人員'

  fixedUnits: GetUnitsResponse[] = [];
  customUnits: GetUnitsResponse[] = [];

  constructor(private httpService: HttpService, public unitService: UnitService) { }

  ngOnInit(): void {
    this.getUnitList();
  }

  async getUnitList() {
    let temp = await this.unitService.getUnits();
    this.fixedUnits = temp[0];
    this.customUnits = temp[1];
  }
}
