import { ReviewNote } from "./review-note";

export class ReviewNoteDialogData {
  editorName?: string;
  createDt?: Date | null;
  notes?: ReviewNote[];

  constructor(editorName?: string, notes?: ReviewNote[], createDt?: Date | null) {
    this.editorName = editorName;
    this.notes = notes;
    this.createDt = createDt;
  }
}
