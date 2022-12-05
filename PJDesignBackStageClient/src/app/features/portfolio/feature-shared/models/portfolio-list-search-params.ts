import { EnabledOptions, EditAndEnabledOptions } from "src/app/shared/models/enums";

export class PortfolioListSearchParams {
  name: string;
  startDt: Date;
  endDt: Date;
  isEnabled = EnabledOptions.全部;
  status = EditAndEnabledOptions.全部;
}
