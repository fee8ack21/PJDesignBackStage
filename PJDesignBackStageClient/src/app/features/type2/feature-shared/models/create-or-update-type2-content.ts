import { EditStatus } from "src/app/shared/models/enums";
import { ReviewNote } from "src/app/shared/models/review-note";

export class CreateOrUpdateType2ContentRequest {
  id?: number | null;
  unitId: number;
  afterId?: number | null;
  title: string;
  isEnabled: boolean;
  content: string;
  editStatus: EditStatus;
  categoryIDs: number[] | null | undefined;
  note?: ReviewNote;
  thumbnailUrl: string;
  imageUrl: string;
}
