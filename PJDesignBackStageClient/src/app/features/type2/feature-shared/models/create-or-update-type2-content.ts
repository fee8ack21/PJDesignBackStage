import { EditRequestBase } from "src/app/shared/models/bases";

export class CreateOrUpdateType2ContentRequest extends EditRequestBase {
  id?: number | null;
  unitId: number;
  title: string;
  description: string;
  isEnabled: boolean;
  isFixed: boolean;
  content: string;
  categoryIDs: number[] | null | undefined;
  thumbnailUrl: string;
  imageUrl: string;
}
