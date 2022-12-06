import { ReviewNote } from "./review-note";

export class GetSettingByUnitIdResponse {
  unitId: number;
  content: object;
  editStatus?: number;
  editorId: number;
  editorName: string;
  reviewId: number;
  notes?: ReviewNote[];
  createDt: Date;
}
