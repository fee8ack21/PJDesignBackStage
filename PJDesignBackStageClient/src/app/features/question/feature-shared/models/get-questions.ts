import { Status } from "src/app/shared/models/enums";

export class GetQuestionsResponse {
  id: number;
  isBefore: boolean;
  name: string;
  categories: Category[];
  createDt: Date;
  editDt: Date;
  editorId: number;
  content: string;
  status: Status;
}

export class Category {
  id: number;
  name: string;
}
