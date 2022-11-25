export class RequestBase {
  PageIndex = 1;
  PageSize = 20;
}

export class ResponseBase<T>{
  TotalItems: number;
}
