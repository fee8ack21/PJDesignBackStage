import { EditRequestBase } from "src/app/shared/models/bases";

export class CreateOrUpdateQuestionRequest extends EditRequestBase {
  id?: number | null;
  title: string;
  isEnabled: boolean;
  content: string;
  categoryIDs: number[] | null | undefined;
}
