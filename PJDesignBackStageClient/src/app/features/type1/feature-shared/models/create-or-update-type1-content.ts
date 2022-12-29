import { EditRequestBase } from "src/app/shared/models/bases";

export class CreateOrUpdateType1ContentRequest extends EditRequestBase {
  unitId?: number | null;
  content: string;
}
