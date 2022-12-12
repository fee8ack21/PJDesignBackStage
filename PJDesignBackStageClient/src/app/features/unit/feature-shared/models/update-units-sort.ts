export class UpdateUnitsSortRequest {
    unitId: number;
    sort: number;

    constructor(unitId: number, sort: number) {
        this.unitId = unitId;
        this.sort = sort;
    }
}
