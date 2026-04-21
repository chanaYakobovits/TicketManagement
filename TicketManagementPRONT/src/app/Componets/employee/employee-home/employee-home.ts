import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-home',
  imports: [ReactiveFormsModule ,CommonModule,FormsModule],
  templateUrl: './employee-home.html',
  styleUrl: './employee-home.css',
})
export class EmployeeHome {
  activeFilter: string = 'all';
  sortBy: string = '';
  currentPage: number = 1;
  totalPages: number = 1;
  searchQuery: string = '';
  totalTickets: number = 0;
  pendingTickets: number = 0;
  resolvedTickets: number = 0;
  urgentTickets: number = 0;
  userInitials: string = 'HN';
  userName: string = 'חנה';
  tickets: any[] = [];

  setFilter(filter: string) {
    this.activeFilter = filter;
    // עדכון רשימת הטיקטים לפי הפילטר
  }

  previousPage() { this.currentPage--; }
  nextPage() { this.currentPage++; }

  viewTicket(id: number) {
    console.log('Viewing ticket', id);
  }

  toggleUserMenu() {}
  toggleNotifications() {}
  onLogout() {}
  onNewTicket() {}
}
