import { EnabledOptions, StatusOptions } from "src/app/shared/models/enums";

export class PortfolioListSearchParams {
  name: string;
  startDt: Date;
  endDt: Date;
  isEnabled = EnabledOptions.全部;
  status = StatusOptions.全部;
}
