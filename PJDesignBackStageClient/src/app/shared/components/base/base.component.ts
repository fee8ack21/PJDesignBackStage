import { Injectable } from '@angular/core';
import { FormControlErrorType, PageStatus, EditStatus } from '../../models/enums';
import { UnitService } from '../../services/unit-service';

@Injectable()
export abstract class BaseComponent {
  unit: { id: number, name: string }

  constructor(protected unitService: UnitService) { }

  public get EditStatus(): typeof EditStatus {
    return EditStatus;
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  setUnit(): void {
    this.unit = this.unitService.getCurrentUnit();
  }

  isUnitInit(): boolean {
    return this.unit.id != null != undefined && this.unit.id != null && this.unit.id != -1;
  }
}
