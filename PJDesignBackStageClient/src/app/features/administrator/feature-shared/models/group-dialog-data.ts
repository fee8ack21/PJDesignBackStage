import { GetBackStageUnitsResponse } from "src/app/shared/models/get-back-stage-units";
import { GetRightsResponse } from "./get-rights";

export interface GroupDialogData {
  name: string;
  units: GetBackStageUnitsResponse[],
  rights: GetRightsResponse[]
}
