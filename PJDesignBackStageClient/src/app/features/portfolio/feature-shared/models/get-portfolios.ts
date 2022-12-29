import { EditResponseBase } from "src/app/shared/models/bases";
import { Category } from "src/app/shared/models/category";

export class GetPortfoliosResponse extends EditResponseBase {
  id: number;
  isBefore: boolean;
  title: string;
  categories: Category[];
  createDt: Date;
  isEnabled: boolean;
  date?: Date;
}
