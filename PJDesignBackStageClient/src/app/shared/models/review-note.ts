export class ReviewNote {
  name: string;
  note: string;
  date?: Date;

  constructor(name: string, note: string, date?: Date) {
    this.name = name;
    this.note = note;
    this.date = date;
  }
}
