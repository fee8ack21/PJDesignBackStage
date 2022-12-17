import { Category } from "src/app/shared/models/category";
import { ReviewNote } from "src/app/shared/models/review-note";

export class GetType2ContentByIdResponse {
  id: number;
  unitId: number;
  thumbnailUrl: string;
  imageUrl: string;
  afterId: number | null | undefined;
  isBefore: boolean;
  title: string;
  createDt: Date;
  editDt: Date;
  editorId: number;
  editorName: string;
  editStatus: number;
  isEnabled: boolean;
  content: string;
  categories: Category[] | null;
  notes?: ReviewNote[];
}
