import { Injectable, Inject } from '@angular/core';
import { Observable, Subject, throwError, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiMessage } from 'src/app/models/ApiMessage';
import { retry, catchError, map } from 'rxjs/operators';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  apiPath: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };
  private authenticationChanged = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiPath = baseUrl + 'api/auth';
    this.authenticationChanged.next(this.isAuthenticated());
  }

  public isAuthenticated(): boolean {
    return (!(window.localStorage.token === undefined ||
              window.localStorage.token === null ||
              window.localStorage.token === 'null' ||
              window.localStorage.token === 'undefined' ||
              window.localStorage.token === ''));
  }

  public isAuthenticationChanged(): Observable<boolean> {
    return this.authenticationChanged.asObservable();
  }

  public getToken(): string {

    if( window.localStorage.token === undefined ||
        window.localStorage.token === null ||
        window.localStorage.token === 'null' ||
        window.localStorage.token === 'undefined' ||
        window.localStorage.token === '') {
        return '';
    }

    return window.localStorage.token;
  }

  public getRole(): string {
    return window.localStorage.role;
  }

  public isAdmin(): boolean {
    return this.getRole() === 'admin';
  }

  public setToken(token: string) {
    window.localStorage.token = token;
    if(this.isAuthenticated()) {
      const decoded = jwt_decode(token);
      window.localStorage.role = decoded.role;
      this.authenticationChanged.next(true);
    } else {
      this.signOut();
    }
  }

  public async signOut() {
    window.localStorage.removeItem('token');
    window.localStorage.removeItem('role');
    this.authenticationChanged.next(false);
  }

  login(name: string, password: string): Observable<boolean> {
    const request = {
      email: name,
      password: password
    };
    return this.http.post<ApiMessage<string>>(this.apiPath, JSON.stringify(request), this.httpOptions).pipe(
      retry(1),
      catchError(this.errorHandler),
      map(response => {
        if (response.error != null) {
          this.setToken(null);
          return false;
        }
        const token = response.data[0];
        this.setToken(token);
        return true;
      })
    );
  }

  errorHandler(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
