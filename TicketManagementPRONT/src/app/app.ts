import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthPages } from './Componets/auth/auth-pages/auth-pages';
// import { EmployeeHome } from './Componets/employee/employee-home/employee-home';
// import { ViewTicket } from './Componets/employee/view-ticket/view-ticket';
// import { NewTicket } from './Componets/employee/new-ticket/new-ticket';
// import { HrHome } from './Componets/hr/hr-home/hr-home';
// import { HandleTicket } from './Componets/hr/handle-ticket/handle-ticket';
// import { ManagerDashboard } from './Componets/manager/manager-dashboard/manager-dashboard';


@Component({
  selector: 'app-root',
  imports: [AuthPages],
  // RouterOutlet
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('TicketManagementPRONT');
}
