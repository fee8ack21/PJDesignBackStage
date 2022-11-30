export class GetBackStageUnitsByGroupIdResponse {
  id: number;
  name: string;
  url?: string;
  isAnotherWindow: boolean;
  parent: number;
  templateType: number;
  children?: GetBackStageUnitsByGroupIdResponse[];
}
