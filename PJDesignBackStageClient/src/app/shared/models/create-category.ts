export class CreateCategoryRequest {
  unitId: number;
  name: string;
  isEnabled: boolean;

  constructor(unitId: number, name: string, isEnabled: boolean) {
    this.unitId = unitId;
    this.name = name;
    this.isEnabled = isEnabled;
  }
}
