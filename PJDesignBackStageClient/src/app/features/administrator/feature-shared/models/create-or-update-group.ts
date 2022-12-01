export class CreateOrUpdateGroupRequest {
  id: number;
  name: string;
  unitRights: UnitRight[] = [];
}

export class UnitRight {
  unitId: number;
  rightId: number;
}
