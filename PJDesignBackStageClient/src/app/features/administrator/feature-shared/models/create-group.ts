export class CreateGroupRequest {
  name: string;

  constructor(name: string) {
    this.name = name;
  }
}

export class CreateGroupResponse {
  id: number;
  name: string;
}
