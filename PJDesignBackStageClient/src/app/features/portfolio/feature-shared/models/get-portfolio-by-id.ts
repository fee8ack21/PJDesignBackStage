import { Category } from "src/app/shared/models/category";
import { ReviewNote } from "src/app/shared/models/review-note";

export class GetPortfolioByIdResponse {
    id: number;
    afterId: number | null | undefined;
    isBefore: boolean;
    title: string;
    createDt: Date;
    editDt: Date;
    editorId: number;
    editorName: string;
    editStatus: number;
    isEnabled: boolean;
    date?: Date;
    categories: Category[] | null;
    notes?: ReviewNote[];
    photos?:string[]
}
