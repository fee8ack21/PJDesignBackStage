import { GetBackStageUnitsResponse } from "src/app/shared/models/get-back-stage-units";
import { GetGroupsResponse } from "./get-groups";
import { GetRightsResponse } from "./get-rights";

export interface GroupDialogData {
  id?: number;
  name?: string;
  units: GetBackStageUnitsResponse[],
  rights: GetRightsResponse[],
  groups?: GetGroupsResponse[],
  isEdit: boolean,
}
