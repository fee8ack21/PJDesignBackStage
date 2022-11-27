import { StatusCode } from "./enums";

export class RequestBase {
}

export class ResponseBase<T>{
  message?: string;
  statusCode?: StatusCode;
  entries?: T;
}

export class ListRequestBase {
  pageIndex?: number;
  pageSize?: number;
}

export class ListResponseBase<T> extends ResponseBase<T> {
  totalItems?: number;
}
