import { Injectable } from '@angular/core';
import { EnabledOptions, FormControlErrorType, PageStatus } from '../../models/enums';

@Injectable()
export abstract class ListBaseComponent {
  enabledOptions = [{ name: '全部', value: EnabledOptions.全部 }, { name: '啟用', value: EnabledOptions.啟用 }, { name: '停用', value: EnabledOptions.停用 }]

  constructor() { }

  public get EnabledOptions(): typeof EnabledOptions {
    return EnabledOptions;
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  getPageStatusName(status: number) {
    switch (status) {
      case PageStatus.Create:
        return '新增';
      case PageStatus.Edit:
        return '編輯';
      case PageStatus.Review:
        return '審核'
      default:
        return '新增'
    }
  }
}
