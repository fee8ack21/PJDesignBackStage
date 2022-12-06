import { Category } from "src/app/shared/models/category";

export class GetQuestionByIdResponse {
  id: number;
  isBefore: boolean;
  title: string;
  createDt: Date;
  editDt: Date;
  editorId: number;
  editStatus: number;
  isEnabled: boolean;
  content: string;
  categories: Category[] | null;
  notes: string;
}
