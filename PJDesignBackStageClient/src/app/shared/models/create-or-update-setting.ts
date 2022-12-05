import { ReviewNote } from "./review-note";

export class CreateOrUpdateSetting {
  unitId: number;
  content: object;
  editStatus: number;
  note?: ReviewNote;
}

