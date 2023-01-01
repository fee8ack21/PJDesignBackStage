import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
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

  extractUrl(url: string | null | undefined): string | null | undefined {
    if (url == null || url == undefined) { return url; }

    return url.split('?')[0];
  }

  extractQueryParams(url: string | null | undefined): { [k: string]: string; } {
    if (url == null || url == undefined || url.split('?').length == 1) { return {}; }

    const params = new URLSearchParams(url.split('?')[1]);
    return Object.fromEntries(params.entries());
  }

  validateForm(form: FormGroup) {
    if (!form.invalid) { return true; }

    form.markAllAsTouched();
    return false;
  }
}
