import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../../Services/auth';
import { UserType } from '../../../models/UserType.model';
import { catchError, Observable, of, tap } from 'rxjs';
import { Department } from '../../../models/Department.model';
import { User } from '../../../models/User.model';

@Component({
  selector: 'app-auth-pages',
  standalone: true, 
  imports: [ReactiveFormsModule ,CommonModule],
  templateUrl: './auth-pages.html',
  // styleUrls: ['./auth-pages.css'],
  styleUrl: './auth-pages.css',
})
export class AuthPages {
  constructor(private fb: FormBuilder , private router: Router) { }
  authService: Auth = inject(Auth);

  loginForm!: FormGroup;
  registerForm!: FormGroup;
  
  showLoginError = false;
  showRegisterSuccess = false;

  list$!: Observable<UserType[]>;
  allType: UserType[] = [];
  list1$!: Observable<Department[]>;
  allDepartment: Department[] = [];

  ngOnInit(): void {
    
    this.initializeForms();
  }

  private initialize():void{
  this.list$ = this.authService.GetUserType().pipe(
    tap(types => {
      this.allType = types;
    }),
    catchError(error => {
      console.error('Error fetching type:', error);
      return of([]);
    })
  );
  this.list$.subscribe();

  this.list1$ = this.authService.GetDepartment().pipe(
    tap(depart => {
      this.allDepartment = depart;
    }),
    catchError(error => {
      console.error('Error fetching depart:', error);
      return of([]);
    })
  );
  this.list1$.subscribe();
}
  /**
   * אתחול טפסים
   */
  private initializeForms(): void {
    // טופס התחברות
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });

    // טופס הרשמה
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      employeeId: ['', [Validators.required]],
      phone: [''],
      department: [[''], [Validators.required]],
      role: [[''], [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
      terms: [false, [Validators.requiredTrue]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  /**
   * ולידטור לבדיקת התאמת סיסמאות
   */
  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword) {
      return null;
    }

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  /**
   * מעבר בין טאבים
   */
  switchTab(tab: 'login' | 'register'): void {
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');
    const tabs = document.querySelectorAll('.tab-button');

    tabs.forEach(button => button.classList.remove('active'));

    if (tab === 'login') {
      loginForm?.classList.add('active');
      registerForm?.classList.remove('active');
      tabs[0].classList.add('active');
      this.showLoginError = false;
    } else {
      this.initialize()
      loginForm?.classList.remove('active');
      registerForm?.classList.add('active');
      tabs[1].classList.add('active');
      this.showRegisterSuccess = false;
    }
  }

  /**
   * טיפול בהתחברות
   */
  onLogin(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const { username, password } = this.loginForm.value;

    // כאן תוסיפי את הקריאה ל-API
    console.log('Login attempt:', { username, password });

    // דוגמה להצגת שגיאה
    // this.showLoginError = true;
    // setTimeout(() => {
    //   this.showLoginError = false;
    // }, 5000);
  }

  /**
   * טיפול בהרשמה
   */
  onRegister(): void {
    if (this.registerForm.invalid) {
      return;
    }

    const formData = this.registerForm.value;
    
    
  const formValue = this.registerForm.value;

  // פיצול שם מלא
  const nameParts = formValue.fullName.trim().split(' ');
  const firstName = nameParts[0];
  const lastName = nameParts.slice(1).join(' '); // תומך גם בשמות עם יותר ממילה אחת

  const user: User = {
    firstName: firstName,
    lastName: lastName,
    email: formValue.email,
    phone: formValue.phone,
    password: formValue.password,
    userTypeId: formValue.role,
    departmentId: formValue.department,
    jobTitl: '', 
    status: true
  };

   
    
      this.authService.addUser(user).subscribe({
        next: () => {
        this.showRegisterSuccess = true;
        this.registerForm.reset();
        setTimeout(() => {
          this.switchTab('login');
        }, 2000);
        },
        error: err => console.error(err)
      });
    

   
  }

  /**
   * טיפול בשכחת סיסמה
   */
  onForgotPassword(event: Event): void {
    event.preventDefault();
    // כאן תוסיפי את הלוגיקה לשחזור סיסמה
    console.log('Forgot password clicked');
  }

  /**
   * טיפול בלחיצה על תנאי שימוש
   */
  onTermsClick(event: Event): void {
    event.preventDefault();
    // כאן תוסיפי את הלוגיקה להצגת תנאי שימוש
    console.log('Terms clicked');
  }

  /**
   * טיפול בלחיצה על מדיניות פרטיות
   */
  onPrivacyClick(event: Event): void {
    event.preventDefault();
    // כאן תוסיפי את הלוגיקה להצגת מדיניות פרטיות
    console.log('Privacy clicked');
  }
}
