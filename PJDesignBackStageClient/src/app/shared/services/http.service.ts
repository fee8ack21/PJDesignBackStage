import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable()

export class HttpService {
  private _httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Cache-Control': 'no-cache, no-store, must-revalidate, post-check=0, pre-check=0',
      'Pragma': 'no-cache',
      'Expires': '0'
      // 'Access-Control-Allow-Origin': 'API Domain',
      // 'Access-Control-Allow-Methods': 'GET, PUT, POST, DELETE, OPTIONS',
      // 'Access-Control-Max-Age': '86400'
    }),
  };

  constructor(private http: HttpClient) { }

  get<T>(path: string, httpOptions = this._httpOptions): Observable<T> {
    return this.http.get<T>(`${environment.apiUrl}${path}`, httpOptions)
  }

  post<T>(path: string, body?: any, httpOptions = this._httpOptions): Observable<T> {
    return this.http.post<T>(`${environment.apiUrl}${path}`, body, httpOptions);
  }

  put<T>(path: string, body?: any, httpOptions = this._httpOptions): Observable<T> {
    return this.http.put<T>(`${environment.apiUrl}${path}`, body, httpOptions);
  }

  patch<T>(path: string, body?: any, httpOptions = this._httpOptions): Observable<T> {
    return this.http.patch<T>(`${environment.apiUrl}${path}`, body, httpOptions);
  }

  delete<T>(path: string, httpOptions = this._httpOptions): Observable<T> {
    return this.http.delete<T>(`${environment.apiUrl}${path}`, httpOptions);
  }

  postPhoto<T>(path: string, file: File) {
    const formData = new FormData();
    formData.append('image', file, file.name);

    return this.http.post<T>(`${environment.apiUrl}${path}`, formData);
  }
}
