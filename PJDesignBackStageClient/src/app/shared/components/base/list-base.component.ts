import { Injectable } from '@angular/core';

@Injectable()
export abstract class ListBaseComponent {
  enabledOptions = [{ name: '全部', value: -1 }, { name: '啟用', value: 1 }, { name: '停用', value: 0 }]

  constructor() { }
}
