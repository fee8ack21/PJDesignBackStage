import { EditStatus, StatusCode } from "./enums";
import { ReviewNote } from "./review-note";

export class ResponseBase<T>{
  message?: string;
  statusCode?: StatusCode;
  entries?: T;
}

export abstract class EditRequestBase {
  afterId?: number | null;
  editStatus: EditStatus;
  note?: ReviewNote;
}

export abstract class EditResponseBase {
  editDt: Date;
  editorId: number;
  editorName: string;
  editStatus: EditStatus;
  notes?: ReviewNote[];
  afterId?: number | null | undefined;
}
