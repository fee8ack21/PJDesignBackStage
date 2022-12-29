import { EditResponseBase } from "./bases";

export class GetSettingByUnitIdResponse extends EditResponseBase {
  unitId: number;
  content: object;
  reviewId: number;
  createDt: Date;
}
