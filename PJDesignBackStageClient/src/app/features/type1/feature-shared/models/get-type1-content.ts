import { ReviewNote } from "src/app/shared/models/review-note";

export class GetType1ContentResponse {
    id: number;
    content: string;
    afterId: number | null | undefined;
    isBefore: boolean;
    createDt: Date;
    editDt: Date;
    editorId: number;
    editorName: string;
    editStatus: number;
    notes?: ReviewNote[];
}
