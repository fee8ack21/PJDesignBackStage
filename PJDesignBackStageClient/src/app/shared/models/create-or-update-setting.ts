import { ReviewNote } from "./review-note";

export class CreateOrUpdateSetting {
  unitId: number;
  content: object;
  status: number;
  note?: ReviewNote;
}

