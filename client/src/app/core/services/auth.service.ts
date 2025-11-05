import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable, BehaviorSubject, tap, catchError, of, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Login } from '../models/login';
import { AuthResponse } from '../models/auth-response';
import { Register } from '../models/register';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {


  public static readonly TOKEN_KEY = 'event_jwt';
  public static readonly EMAIL_KEY = 'event_user_email';
  public static readonly USERNAME_KEY = 'event_username';

  private apiUrl = `${environment.apiUrl}/Users`;

  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);
  private isBrowser = isPlatformBrowser(this.platformId);

  private router = inject(Router);


  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isBrowser ? this.hasToken() : false);
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  private currentUserEmail = new BehaviorSubject<string>(this.isBrowser ? this.getUserEmail() : '');
  currentUserEmail$ = this.currentUserEmail.asObservable();

  private currentUsername = new BehaviorSubject<string>(this.isBrowser ? this.getUsername() : '');
  currentUsername$ = this.currentUsername.asObservable();



  private hasToken(): boolean {
    return !!localStorage.getItem(AuthService.TOKEN_KEY);
  }

  private getUserEmail(): string {
    return localStorage.getItem(AuthService.EMAIL_KEY) || '';
  }

  private getUsername(): string {
    return localStorage.getItem(AuthService.USERNAME_KEY) || '';
  }

  public getToken(): string | null {
    if (this.isBrowser) {
      return localStorage.getItem(AuthService.TOKEN_KEY);
    }
    return null;
  }

  login(dto: Login): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => {

        if (res.token && res.email && res.username) {
          this.setSession(res.token, res.email, res.username);
        }
      }),

      catchError(error => {
        return throwError(() => error);
      })
    );
  }

  register(dto: Register): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, dto).pipe(
      tap(
        //res => {
       // if (res.token && res.email && res.username) {
      //    this.setSession(res.token, res.email, res.username);
     //   }}
     ),
      catchError(error => {
        return throwError(() => error);
      })
    );
  }



  private setSession(token: string, email: string, username: string): void {
    if (this.isBrowser) {

      localStorage.setItem(AuthService.TOKEN_KEY, token);
      localStorage.setItem(AuthService.EMAIL_KEY, email);
      localStorage.setItem(AuthService.USERNAME_KEY, username);
      this.isLoggedInSubject.next(true);
      this.currentUserEmail.next(email);
      this.currentUsername.next(username);
    }
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem(AuthService.TOKEN_KEY);
      localStorage.removeItem(AuthService.EMAIL_KEY);
      localStorage.removeItem(AuthService.USERNAME_KEY);
      this.isLoggedInSubject.next(false);
      this.currentUserEmail.next('');
      this.currentUsername.next('');

      this.router.navigate(['/login']);
    }
  }
}