import { Routes } from '@angular/router';
import { AuthPages } from './Componets/auth/auth-pages/auth-pages';
import { ResetPassword  } from './Componets/auth/reset-password/reset-password';
import { authGuard } from './Services/auth.guard';
import { EmployeeHome } from './Componets/employee/employee-home/employee-home';
import { NewTicket } from './Componets/employee/new-ticket/new-ticket';
import { ViewTicket } from './Componets/employee/view-ticket/view-ticket';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  
  // דפים פתוחים — ללא Guard
  { path: 'auth', component: AuthPages },
  { path: 'reset-password', component: ResetPassword },
  {
    path: 'employee',
    children: [
      { path: '', component: EmployeeHome },
      { path: 'new-ticket', component: NewTicket },
      { path: 'view-ticket/:id', component: ViewTicket },
    ]
  },
  { path: '**', redirectTo: 'auth' }
];