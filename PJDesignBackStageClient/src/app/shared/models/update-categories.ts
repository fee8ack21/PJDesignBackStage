export class UpdateCategoriesRequest {
  id: number;
  name: string;
  isEnabled: boolean;

  constructor(id: number, name: string, isEnabled: boolean) {
    this.id = id;
    this.name = name;
    this.isEnabled = isEnabled;
  }
}
