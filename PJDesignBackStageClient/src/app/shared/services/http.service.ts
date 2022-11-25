import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable()

export class HttpService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      // 'Access-Control-Allow-Origin': 'API Domain',
      // 'Access-Control-Allow-Methods': 'GET, PUT, POST, DELETE, OPTIONS',
      // 'Access-Control-Max-Age': '86400'
    }),
  };

  constructor(private http: HttpClient) { }

  get<T>(path: string): Observable<T> {
    return this.http.get<T>(`${environment.apiUrl}${path}`, this.httpOptions)
  }

  post<T>(path: string, body?: any): Observable<T> {
    return this.http.post<T>(`${environment.apiUrl}${path}`, body, this.httpOptions);
  }

  put<T>(path: string, body?: any): Observable<T> {
    return this.http.put<T>(`${environment.apiUrl}${path}`, body, this.httpOptions);
  }

  patch<T>(path: string, body?: any): Observable<T> {
    return this.http.patch<T>(`${environment.apiUrl}${path}`, body, this.httpOptions);
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${environment.apiUrl}${path}`, this.httpOptions);
  }
}
