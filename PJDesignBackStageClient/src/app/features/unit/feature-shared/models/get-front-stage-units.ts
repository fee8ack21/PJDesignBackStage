import { StageType } from "src/app/shared/models/enums";

export class GetFrontStageUnits {
  id: number;
  name: string;
  templateType: number;
  isAnotherWindow: boolean;
  isEnabled?: boolean;
  createDt: Date;
  parent?: number;
  stageType: StageType;
  sort?: number;
}
