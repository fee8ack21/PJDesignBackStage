import { EditRequestBase } from "src/app/shared/models/bases";

export class CreateOrUpdateType2ContentRequest extends EditRequestBase {
  id?: number | null;
  unitId: number;
  title: string;
  isEnabled: boolean;
  content: string;
  categoryIDs: number[] | null | undefined;
  thumbnailUrl: string;
  imageUrl: string;
}
