import { EditStatus } from "./enums";
import { ReviewNote } from "./review-note";

export class CreateOrUpdateSettingRequest {
  unitId: number;
  content: object;
  editStatus: EditStatus | undefined;
  note?: ReviewNote;

  constructor(unitId: number, content: object, editStatus: EditStatus | undefined, note?: ReviewNote) {
    this.unitId = unitId;
    this.content = content;
    this.editStatus = editStatus;
    this.note = note;
  }
}

