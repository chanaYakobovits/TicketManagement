import { Routes } from '@angular/router';
import { AuthPages } from './Componets/auth/auth-pages/auth-pages';
import { ResetPassword  } from './Componets/auth/reset-password/reset-password';
import { authGuard } from './Services/auth.guard';
import { EmployeeHome } from './Componets/employee/employee-home/employee-home';
import { NewTicket } from './Componets/employee/new-ticket/new-ticket';
import { ViewTicket } from './Componets/employee/view-ticket/view-ticket';
import { HandleTicket } from './Componets/hr/handle-ticket/handle-ticket';
import { HrHome } from './Componets/hr/hr-home/hr-home';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
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
  {
    path: 'hr',
    children: [
      { path: '', component: HrHome },
      { path: 'handle-ticket/:id', component: HandleTicket },
    ]
  },
  { path: '**', redirectTo: 'auth' }
];