import { GetCategoriesByUnitId } from "./get-categories-by-unit-id";

export class CategoryDialogData {
  unitId: number;
  isEdit: boolean;
  categories?: GetCategoriesByUnitId[];

  constructor(unitId: number, isEdit: boolean, categories?: GetCategoriesByUnitId[]) {
    this.unitId = unitId;
    this.isEdit = isEdit;
    this.categories = categories;
  }
}
