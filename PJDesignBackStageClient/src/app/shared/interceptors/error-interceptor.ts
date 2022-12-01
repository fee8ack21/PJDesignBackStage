import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from "rxjs/operators";
import { SnackBarService } from '../services/snack-bar.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private snackBarService: SnackBarService, private authService: AuthService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(1),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = '';

          if (error.error instanceof ErrorEvent) {
            // client side
            errorMessage = `Error: ${error.error.message}`;
          } else {
            // server side
            errorMessage = `Error Code: ${error.status},  Message: ${error.message}`;
          }

          if (error.status == 401) {
            this.authService.removeToken();
            this.router.navigate(['/']);
          }

          console.log(errorMessage);

          this.snackBarService.showSnackBar(SnackBarService.RequestFailedText);

          return throwError(errorMessage);
        })
      )
  }
}
