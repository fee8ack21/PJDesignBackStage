import { EditStatus } from "src/app/shared/models/enums";
import { ReviewNote } from "src/app/shared/models/review-note";

export class CreateOrUpdatePortfolioRequest {
  id?: number | null;
  afterId?: number | null;
  title: string;
  isEnabled: boolean;
  date: Date;
  editStatus: EditStatus;
  categoryIDs: number[] | null | undefined;
  note?: ReviewNote;
  photos?: string[]
}
