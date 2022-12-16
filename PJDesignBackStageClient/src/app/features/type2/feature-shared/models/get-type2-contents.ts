import { Category } from "src/app/shared/models/category";
import { EditStatus } from "src/app/shared/models/enums";

export class GetType2ContentsResponse {
  id: number;
  isBefore: boolean;
  title: string;
  categories: Category[];
  createDt: Date;
  editDt: Date;
  editorId: number;
  editStatus: EditStatus;
  isEnabled: boolean;
}
