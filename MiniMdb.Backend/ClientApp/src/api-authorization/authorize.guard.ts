import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { tap } from 'rxjs/operators';
import { ApplicationPaths, QueryParameterNames } from './api-authorization.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeGuard implements CanActivate {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      if(!this.authorize.isAuthenticated()) {
        this.router.navigate(['/login']);
        return false;
      }
      return true;
  }

  private handleAuthorization(isAuthenticated: boolean, state: RouterStateSnapshot) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents, {
        queryParams: {
          [QueryParameterNames.ReturnUrl]: state.url
        }
      });
    }
  }
}
