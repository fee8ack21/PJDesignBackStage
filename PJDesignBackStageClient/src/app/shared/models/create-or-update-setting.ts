import { EditRequestBase } from "./bases";
import { EditStatus } from "./enums";
import { ReviewNote } from "./review-note";

export class CreateOrUpdateSettingRequest extends EditRequestBase {
  unitId: number;
  content: object;

  constructor(unitId: number, content: object, editStatus: EditStatus, note?: ReviewNote) {
    super();
    this.unitId = unitId;
    this.content = content;
    this.editStatus = editStatus;
    this.note = note;
  }
}

