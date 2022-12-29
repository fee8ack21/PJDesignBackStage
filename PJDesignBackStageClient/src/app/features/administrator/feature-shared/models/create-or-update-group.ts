export class CreateOrUpdateGroupRequest {
  id: number;
  name: string;
  unitRights: UnitRight[] = [];

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

export class UnitRight {
  unitId: number;
  rightId: number;

  constructor(unitId: number, rightId: number) {
    this.unitId = unitId;
    this.rightId = rightId;
  }
}
