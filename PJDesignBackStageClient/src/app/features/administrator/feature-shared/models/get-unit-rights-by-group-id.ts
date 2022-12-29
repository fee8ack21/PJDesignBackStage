export class GetUnitRightsByGroupIdRequest {
  id: number;

  constructor(id: number) {
    this.id = id;
  }
}

export class GetUnitRightsByGroupIdResponse {
  unitId: number;
  rightId: number;
}
