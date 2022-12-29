export class ContactDialogData {
  name: string;
  createDt: Date;
  content: string;

  constructor(name: string, content: string, createDt: Date) {
    this.name = name;
    this.content = content;
    this.createDt = createDt;
  }
}
