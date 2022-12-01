import { EnabledOptions } from "src/app/shared/models/enums";

export class AdministratorListSearchParams {
  name: string;
  groupId = -1;
  startDt: Date;
  endDt: Date;
  isEnabled = EnabledOptions.全部;
}
