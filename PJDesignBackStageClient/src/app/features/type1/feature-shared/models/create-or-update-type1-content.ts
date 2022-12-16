import { EditStatus } from "src/app/shared/models/enums";
import { ReviewNote } from "src/app/shared/models/review-note";

export class CreateOrUpdateType1ContentRequest {
    unitId?: number | null;
    content: string;
    editStatus: EditStatus;
    note?: ReviewNote;
}
