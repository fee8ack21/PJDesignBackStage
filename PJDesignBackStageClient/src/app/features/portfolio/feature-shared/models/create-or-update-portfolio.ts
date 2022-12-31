import { EditRequestBase } from "src/app/shared/models/bases";

export class CreateOrUpdatePortfolioRequest extends EditRequestBase {
  id?: number | null;
  title: string;
  isEnabled: boolean;
  date: Date;
  categoryIDs: number[] | null | undefined;
  photos?: string[]
  thumbnailUrl: string;
}
