import { Injectable } from '@angular/core';
import { EnabledOptions, FormControlErrorType, PageStatus, EditStatus, EditAndEnabledOptions } from '../../models/enums';

@Injectable()
export abstract class ListBaseComponent {
  enabledOptions = [{ name: '全部', value: EnabledOptions.全部 }, { name: '啟用', value: EnabledOptions.啟用 }, { name: '停用', value: EnabledOptions.停用 }]
  editAndEnabledOptions = [{ name: '全部', value: EditAndEnabledOptions.全部 }, { name: '啟用', value: EditAndEnabledOptions.啟用 }, { name: '停用', value: EditAndEnabledOptions.停用 }, { name: '審核中', value: EditAndEnabledOptions.審核中 }, { name: '駁回', value: EditAndEnabledOptions.駁回 }]

  constructor() { }

  public get EditStatus(): typeof EditStatus {
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

  getEnabledOptionName(value: boolean | number) {
    return value ? '啟用' : '停用';
  }

  getEditAndEnabledStatusName(status: boolean | number) {
    if (typeof status == 'boolean') {
      if (status) {
        return '啟用';
      }

      return '停用'
    }

    switch (status) {
      case EditStatus.Review:
        return '審核中';
      case EditStatus.Reject:
        return '駁回';
      default:
        return '審核中';
    }
  }
}
