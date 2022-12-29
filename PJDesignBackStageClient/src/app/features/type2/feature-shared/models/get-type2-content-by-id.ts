import { EditResponseBase } from "src/app/shared/models/bases";
import { Category } from "src/app/shared/models/category";

export class GetType2ContentByIdResponse extends EditResponseBase {
  id: number;
  unitId: number;
  thumbnailUrl: string;
  imageUrl: string;
  isBefore: boolean;
  title: string;
  createDt: Date;
  isEnabled: boolean;
  content: string;
  categories: Category[] | null;
}
