import { EditStatus } from "src/app/shared/models/enums";

export class CreateOrUpdateQuestionRequest {
  id?: number | null;
  title: string;
  isEnabled: boolean;
  content: string;
  editStatus: EditStatus;
  categoryIDs: number[] | null | undefined;
  note?: string;
}
