import { EditResponseBase } from "src/app/shared/models/bases";
import { Category } from "src/app/shared/models/category";

export class GetQuestionByIdResponse extends EditResponseBase {
  id: number;
  isBefore: boolean;
  title: string;
  createDt: Date;
  isEnabled: boolean;
  content: string;
  categories: Category[] | null;
}
