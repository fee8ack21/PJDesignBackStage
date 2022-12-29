import { EditResponseBase } from "src/app/shared/models/bases";
import { Category } from "src/app/shared/models/category";

export class GetPortfolioByIdResponse extends EditResponseBase {
  id: number;
  isBefore: boolean;
  title: string;
  createDt: Date;
  isEnabled: boolean;
  date?: Date;
  categories: Category[] | null;
  photos?: string[]
}
