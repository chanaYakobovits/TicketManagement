import { Routes } from '@angular/router';
import { AuthPages } from './Componets/auth/auth-pages/auth-pages';
import { ResetPassword  } from './Componets/auth/reset-password/reset-password';
import { authGuard } from './Services/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  
  // דפים פתוחים — ללא Guard
  { path: 'auth', component: AuthPages },
  { path: 'reset-password', component: ResetPassword },
  { path: '**', redirectTo: 'auth' }
];