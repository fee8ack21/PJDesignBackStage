import { Injectable } from '@angular/core';
import { EnabledOptions, FormControlErrorType, PageStatus, EditStatus, StatusOptions } from '../../models/enums';

@Injectable()
export abstract class ListBaseComponent {
  enabledOptions = [{ name: '全部', value: EnabledOptions.全部 }, { name: '啟用', value: EnabledOptions.啟用 }, { name: '停用', value: EnabledOptions.停用 }]
  statusOptions = [{ name: '全部', value: StatusOptions.全部 }, { name: '啟用', value: StatusOptions.啟用 }, { name: '停用', value: StatusOptions.停用 }, { name: '審核中', value: StatusOptions.審核中 }, { name: '駁回', value: StatusOptions.駁回 }]

  constructor() { }

  public get Status(): typeof EditStatus {
    return EditStatus;
  }

  public get EnabledOptions(): typeof EnabledOptions {
    return EnabledOptions;
  }

  public get PageStatus(): typeof PageStatus {
    return PageStatus;
  }

  public get FormControlErrorType(): typeof FormControlErrorType {
    return FormControlErrorType;
  }

  getEnabledOptionName(value: boolean | number) {
    return value ? '啟用' : '停用';
  }

  getStatusOptionName(value: number) {
    switch (value) {
      case StatusOptions.全部:
        return '全部';
      case StatusOptions.啟用:
        return '啟用';
      case StatusOptions.停用:
        return '停用';
      case StatusOptions.審核中:
        return '審核中';
      case StatusOptions.駁回:
        return '駁回';
      default:
        return '';
    }
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
