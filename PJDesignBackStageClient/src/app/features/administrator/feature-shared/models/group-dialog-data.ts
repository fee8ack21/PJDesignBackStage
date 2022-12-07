import { GetUnitsResponse } from "src/app/shared/models/get-units";
import { GetGroupsResponse } from "./get-groups";
import { GetRightsResponse } from "./get-rights";

export interface GroupDialogData {
  id?: number;
  name?: string;
  units: GetUnitsResponse[],
  rights: GetRightsResponse[],
  groups?: GetGroupsResponse[],
  isEdit: boolean,
}
