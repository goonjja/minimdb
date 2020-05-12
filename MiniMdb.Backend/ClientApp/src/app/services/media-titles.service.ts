import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MediaTitle } from '../models/MediaTitles';
import { ApiMessage } from '../models/ApiMessage';

@Injectable({
  providedIn: 'root'
})
export class MediaTitlesService {

  apiPath: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiPath = baseUrl + 'api/mediatitles';
  }

  getMediaTitles(page: number = 1, pageSize: number = 5): Observable<ApiMessage<MediaTitle>> {
    const params = new HttpParams()
    .set('page', page.toString())
    .set('pageSize', pageSize.toString());

    return this.http.get<ApiMessage<MediaTitle>>(this.apiPath, {params})
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  errorHandler(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
