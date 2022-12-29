import { StatusCode } from "./enums";

export class ResponseBase<T>{
  message?: string;
  statusCode?: StatusCode;
  entries?: T;
}
