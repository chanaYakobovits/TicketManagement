import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Ticket } from '../../../Services/ticket';


@Component({
  selector: 'app-employee-home',
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './employee-home.html',
  styleUrl: './employee-home.css',
})
export class EmployeeHome implements OnInit {
  activeFilter: string = 'all';
  sortBy: string = 'date';
  currentPage: number = 1;
  itemsPerPage: number = 10;
  searchQuery: string = '';
  userInitials: string = '';
  userName: string = '';
  currentUserId: number = 0;

  allTickets: any[] = [];   // כל הטיקטים מהשרת
  tickets: any[] = [];       // הטיקטים המוצגים אחרי פילטר/חיפוש

  // סטטיסטיקות
  get totalTickets() { return this.allTickets.length; }
  get pendingTickets() { return this.allTickets.filter(t => t.currentStatus === 'פתוח' || t.currentStatus === 'בטיפול').length; }
  get resolvedTickets() { return this.allTickets.filter(t => t.currentStatus === 'סגור').length; }
  get urgentTickets() { return this.allTickets.filter(t => t.urgencyLevel >= 4).length; }

  // פאגינציה
  get totalPages() { return Math.ceil(this.tickets.length / this.itemsPerPage) || 1; }
  get paginatedTickets() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    return this.tickets.slice(start, start + this.itemsPerPage);
  }

  constructor(private router: Router, private ticketService: Ticket) {}

  ngOnInit(): void {
    const raw = localStorage.getItem('user');
    if (raw) {
      const user = JSON.parse(raw);
      this.currentUserId = user.id;
      this.userName = `${user.firstName} ${user.lastName}`;
      this.userInitials = `${user.firstName?.charAt(0) ?? ''}${user.lastName?.charAt(0) ?? ''}`;
    }
    this.loadTickets();
  }

  loadTickets() {
    this.ticketService.getTicketsByUser(this.currentUserId).subscribe({
      next: (data) => {
        this.allTickets = data;
        this.applyFilters();
      },
      error: () => console.error('שגיאה בטעינת הפניות')
    });
  }

  applyFilters() {
    let filtered = [...this.allTickets];

    // פילטר סטטוס
    if (this.activeFilter === 'open')
      filtered = filtered.filter(t => t.currentStatus === 'פתוח');
    else if (this.activeFilter === 'pending')
      filtered = filtered.filter(t => t.currentStatus === 'בטיפול');
    else if (this.activeFilter === 'resolved')
      filtered = filtered.filter(t => t.currentStatus === 'סגור');

    // חיפוש
    if (this.searchQuery.trim())
      filtered = filtered.filter(t =>
        t.subject?.includes(this.searchQuery) ||
        t.description?.includes(this.searchQuery)
      );

    // מיון
    if (this.sortBy === 'date')
      filtered.sort((a, b) => new Date(b.openedDate).getTime() - new Date(a.openedDate).getTime());
    else if (this.sortBy === 'status')
      filtered.sort((a, b) => a.currentStatus?.localeCompare(b.currentStatus));
    else if (this.sortBy === 'urgency')
      filtered.sort((a, b) => b.urgencyLevel - a.urgencyLevel);

    this.tickets = filtered;
    this.currentPage = 1;
  }

  setFilter(filter: string) {
    this.activeFilter = filter;
    this.applyFilters();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'פתוח': return 'status-open';
      case 'בטיפול': return 'status-pending';
      case 'סגור': return 'status-resolved';
      default: return '';
    }
  }

  getUrgencyLabel(level: number): string {
    switch (level) {
      case 1: return 'נמוכה';
      case 2: return 'בינונית';
      case 3: return 'גבוהה';
      case 4: return 'קריטית';
      default: return 'לא ידוע';
    }
  }

  getUrgencyClass(level: number): string {
    switch (level) {
      case 1: return 'urgency-low';
      case 2: return 'urgency-medium';
      case 3: return 'urgency-high';
      case 4: return 'urgency-critical';
      default: return '';
    }
  }

  previousPage() { if (this.currentPage > 1) this.currentPage--; }
  nextPage() { if (this.currentPage < this.totalPages) this.currentPage++; }
  viewTicket(id: number) { this.router.navigate(['/employee/view-ticket', id]); }
  onNewTicket() { this.router.navigate(['/employee/new-ticket']); }
  toggleUserMenu() {}
  toggleNotifications() {}
  onLogout() {
    localStorage.removeItem('user');
    this.router.navigate(['/auth']);
  }
}