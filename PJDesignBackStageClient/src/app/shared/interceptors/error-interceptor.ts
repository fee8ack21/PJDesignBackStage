import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from "rxjs/operators";
import { SnackBarService } from '../services/snack-bar.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private snackBarService: SnackBarService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(1),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = '';

          if (error.error instanceof ErrorEvent) {
            console.log('This is client side error');
            errorMessage = `Error: ${error.error.message}`;
          } else {
            console.log('This is server side error');
            errorMessage = `Error Code: ${error.status},  Message: ${error.message}`;
          }
          console.log(errorMessage);

          this.snackBarService.showSnackBar('請求錯誤');

          return throwError(errorMessage);
        })
      )
  }
}
