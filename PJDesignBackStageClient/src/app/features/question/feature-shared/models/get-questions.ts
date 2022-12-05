import { EditStatus } from "src/app/shared/models/enums";

export class GetQuestionsResponse {
  id: number;
  isBefore: boolean;
  title: string;
  categories: Category[];
  createDt: Date;
  editDt: Date;
  editorId: number;
  content: string;
  status: EditStatus;
}

export class Category {
  id: number;
  name: string;
}
