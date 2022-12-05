import { GetCategoriesByUnitId } from "./get-categories-by-unit-id";

export class CategoryDialogData {
  unitId: number;
  isEdit: boolean;
  categories?: GetCategoriesByUnitId[];
}
