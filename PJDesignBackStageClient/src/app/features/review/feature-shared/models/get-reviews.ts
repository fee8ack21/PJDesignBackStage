export class GetReviewsResponse {
  unitId: number;
  unitName: string;
  templateType: number | null | undefined;
  title?: string;
  url?: string;
  contentId: number;
  editDt: Date;
  editStatus: number;
  editorId: number;
  editorName: string;
}
