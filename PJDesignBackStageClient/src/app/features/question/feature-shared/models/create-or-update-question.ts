import { EditStatus } from "src/app/shared/models/enums";
import { ReviewNote } from "src/app/shared/models/review-note";

export class CreateOrUpdateQuestionRequest {
  id?: number | null;
  afterId?: number | null;
  title: string;
  isEnabled: boolean;
  content: string;
  editStatus: EditStatus;
  categoryIDs: number[] | null | undefined;
  note?: ReviewNote;
}
