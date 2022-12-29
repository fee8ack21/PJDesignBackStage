import { EditResponseBase } from "src/app/shared/models/bases";

export class GetReviewsResponse extends EditResponseBase {
  unitId: number;
  unitName: string;
  templateType: number | null | undefined;
  title?: string;
  url?: string;
  contentId: number;
}
