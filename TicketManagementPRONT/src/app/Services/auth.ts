import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { User } from '../models/User.model';
import { environment } from "../env/environment.dev";
import { UserType } from '../models/UserType.model';
import { Department } from '../models/Department.model';
@Injectable({ 
  providedIn: 'root',
})
export class Auth {
  baseUrl = environment.URLS.apiUrl;
  BASE_URL = this.baseUrl + 'User'

  http: HttpClient = inject(HttpClient);

  constructor() { }


  GetUser(): Observable<User[]> {
    console.log('🔹 Fetching requests from:', this.BASE_URL);
    return this.http.get<User[]>(`${this.BASE_URL}`, { withCredentials: true }).pipe(
      tap((data) => console.log('✅ Data received:', data)), // לוודא שהנתונים באמת מגיעים
      catchError(error => {
        console.error('❌ Error fetching managers:', error);
        return of([]);
      })
    );
  }

  GetUserType(): Observable<UserType[]> {
    console.log('🔹 Fetching type from:', this.BASE_URL+'/UserType/');
    return this.http.get<UserType[]>(`${this.BASE_URL}/UserType/`).pipe(
      tap((data) => console.log('✅ Data received:', data)), // לוודא שהנתונים באמת מגיעים
      catchError(error => {
        console.error('❌ Error fetching managers:', error);
        return of([]);
      })
    );
  }
     
  GetDepartment(): Observable<Department[]> {
    console.log('🔹 Fetching type from:', this.BASE_URL+'/Department/');
    return this.http.get<Department[]>(`${this.BASE_URL}/Department/`).pipe(
      tap((data) => console.log('✅ Data received:', data)), // לוודא שהנתונים באמת מגיעים
      catchError(error => {
        console.error('❌ Error fetching managers:', error);
        return of([]);
      })
    );
  }
 addUser(user: User): Observable<any> {
    console.log('🔹 Sending applicant details to:', this.BASE_URL, user);

    return this.http.post<any>(`${this.BASE_URL}`, user).pipe(
      tap((data) => console.log('✅ Data received:', data)),
      catchError(error => {
        console.error('❌ Error sending applicant:', error);
        throw error; // ← שנה מ-return of([]) ל-throw error
      })
    );
}

 login(credentials: { employeeId: string, password: string }): Observable<any> {
  return this.http.post<any>(`${this.BASE_URL}/login`, credentials).pipe(
    tap(response => {
      if (response?.success) {
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', JSON.stringify(response.data));
      }
    })
  );
}

logout(): void {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
}

getToken(): string | null {
  return localStorage.getItem('token');
}

isLoggedIn(): boolean {
  return !!this.getToken();
}

forgotPassword(email: string): Observable<any> {
  return this.http.post<any>(`${this.BASE_URL}/forgot-password`, { email }).pipe(
    tap((data) => console.log('✅ Forgot password:', data)),
    catchError(error => {
      console.error('❌ Forgot password error:', error);
      throw error;
    })
  );
}

resetPassword(token: string, newPassword: string): Observable<any> {
  return this.http.post<any>(`${this.BASE_URL}/reset-password`, { token, newPassword }).pipe(
    tap((data) => console.log('✅ Reset password:', data)),
    catchError(error => {
      console.error('❌ Reset password error:', error);
      throw error;
    })
  );
}

}
