import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { User } from '@app/models/identity/User';
import { catchError, take } from 'rxjs/operators';
import { AccountService } from '@app/services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currenUser: User;

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      currenUser = user

      if(currenUser){
        request = request.clone(
          {
            setHeaders: {
              Authorization: `Bearer ${currenUser.token}`
            }
          }
        );
      }
    });

    return next.handle(request).pipe(
      catchError(error => {
        if(error){
          localStorage.removeItem('user')
        }
        return throwError(error);
      }
    )
  );
  }
}
