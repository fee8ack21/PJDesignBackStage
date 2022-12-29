import { EditResponseBase } from "src/app/shared/models/bases";
import { ReviewNote } from "src/app/shared/models/review-note";

export class GetType1ContentResponse extends EditResponseBase {
  id: number;
  content: string;
  isBefore: boolean;
  createDt: Date;
}
