import { EditStatus } from "./enums";
import { ReviewNote } from "./review-note";

export class CreateOrUpdateSettingRequest {
  unitId: number;
  content: object;
  editStatus: EditStatus | undefined;
  note?: ReviewNote;
}

