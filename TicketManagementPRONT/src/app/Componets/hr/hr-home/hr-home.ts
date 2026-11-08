import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Ticket } from '../../../Services/ticket';


@Component({
  selector: 'app-hr-home',
  imports: [CommonModule, FormsModule],
  templateUrl: './hr-home.html',
  styleUrl: './hr-home.css',
})

export class HrHome implements OnInit {
  userInitials = '';
  userName = '';
  currentUserId = 0;
  notificationCount = 0;

  view: 'queue' | 'kanban' = 'queue';
  filterStatus = 'all';
  searchQuery = '';
  sortBy = 'urgency';

  allTickets: any[] = [];
  tickets: any[] = [];

  // סטטיסטיקות
  get assignedCount() { return this.allTickets.length; }
  get urgentCount() { return this.allTickets.filter(t => t.urgencyLevel >= 3).length; }
  get pendingCount() { return this.allTickets.filter(t => t.currentStatus === 'בטיפול').length; }
  get pendingProgress() {
    return this.allTickets.length > 0
      ? Math.round((this.pendingCount / this.allTickets.length) * 100)
      : 0;
  }
  get resolvedTodayCount() {
    const today = new Date().toISOString().split('T')[0];
    return this.allTickets.filter(t => t.currentStatus === 'סגור' && t.openedDate === today).length;
  }
  get newCount() { return this.allTickets.filter(t => t.currentStatus === 'פתוח').length; }
  get inProgressCount() { return this.allTickets.filter(t => t.currentStatus === 'בטיפול').length; }
  get totalCount() { return this.allTickets.length; }
  avgResponseTime = 'לא ידוע';
  satisfactionScore = 0;
  satisfactionStars = 0;

  // פאגינציה
  currentPage = 1;
  pageSize = 10;
  get totalPages() { return Math.ceil(this.tickets.length / this.pageSize) || 1; }
  get totalItems() { return this.tickets.length; }
  get startItem() { return this.totalItems === 0 ? 0 : (this.currentPage - 1) * this.pageSize + 1; }
  get endItem() { return Math.min(this.currentPage * this.pageSize, this.totalItems); }
  get pages() { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }
  get paginatedTickets() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.tickets.slice(start, start + this.pageSize);
  }

  selectAll = false;

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

  loadTickets(): void {
    this.ticketService.getAssignedTickets(this.currentUserId).subscribe({
      next: (data) => {
        this.allTickets = data.map(t => ({
          ...t,
          selected: false,
          employeeName: `${t.openedByUser?.firstName ?? ''} ${t.openedByUser?.lastName ?? ''}`,
          employeeInitials: `${t.openedByUser?.firstName?.charAt(0) ?? ''}${t.openedByUser?.lastName?.charAt(0) ?? ''}`,
          department: t.openedByUser?.jobTitle ?? '',
          urgencyText: this.getUrgencyLabel(t.urgencyLevel),
          urgencyClass: this.getUrgencyClass(t.urgencyLevel),
          category: t.category?.categoryName ?? 'לא ידוע',
          categoryClass: 'cat-default',
          aiConfidence: t.aianalyses?.[0] ? 90 : 0,
          preview: t.description?.substring(0, 50) + '...',
          riskLevel: this.getRiskClass(t.aianalyses?.[0]?.riskLevel),
          riskScore: this.getRiskScore(t.aianalyses?.[0]?.riskLevel),
          riskLabel: t.aianalyses?.[0]?.riskLevel ?? 'לא ידוע',
          slaStatus: 'sla-ok',
          slaRemaining: 'לא ידוע',
          slaPercentage: 0,
          statusClass: this.getStatusClass(t.currentStatus)
        }));
        this.applyFilters();
      },
      error: () => console.error('שגיאה בטעינת הפניות')
    });
  }

  applyFilters(): void {
    let filtered = [...this.allTickets];

    if (this.filterStatus === 'urgent')
      filtered = filtered.filter(t => t.urgencyLevel >= 3);
    else if (this.filterStatus === 'new')
      filtered = filtered.filter(t => t.currentStatus === 'פתוח');
    else if (this.filterStatus === 'inProgress')
      filtered = filtered.filter(t => t.currentStatus === 'בטיפול');

    if (this.searchQuery.trim())
      filtered = filtered.filter(t =>
        t.subject?.includes(this.searchQuery) ||
        t.employeeName?.includes(this.searchQuery)
      );

    if (this.sortBy === 'urgency')
      filtered.sort((a, b) => b.urgencyLevel - a.urgencyLevel);
    else if (this.sortBy === 'date')
      filtered.sort((a, b) => new Date(b.openedDate).getTime() - new Date(a.openedDate).getTime());

    this.tickets = filtered;
    this.currentPage = 1;
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
      case 1: return 'נמוכה'; case 2: return 'בינונית';
      case 3: return 'גבוהה'; case 4: return 'קריטית';
      default: return 'לא ידוע';
    }
  }

  getUrgencyClass(level: number): string {
    switch (level) {
      case 1: return 'urgency-low'; case 2: return 'urgency-medium';
      case 3: return 'urgency-high'; case 4: return 'urgency-critical';
      default: return '';
    }
  }

  getRiskClass(risk: string): string {
    switch (risk) {
      case 'Low': return 'risk-low';
      case 'Medium': return 'risk-medium';
      case 'High': return 'risk-high';
      default: return '';
    }
  }

  getRiskScore(risk: string): number {
    switch (risk) {
      case 'Low': return 2; case 'Medium': return 5; case 'High': return 8;
      default: return 0;
    }
  }

  setView(view: 'queue' | 'kanban') { this.view = view; }
  setFilter(status: string) { this.filterStatus = status; this.applyFilters(); }
  refreshTickets() { this.loadTickets(); }
  toggleSelectAll() { this.tickets.forEach(t => t.selected = this.selectAll); }
  hasSelectedTickets() { return this.tickets.some(t => t.selected); }
  selectedCount() { return this.tickets.filter(t => t.selected).length; }
  handleTicket(id: number) { this.router.navigate(['/hr/handle-ticket', id]); }
  viewDetails(id: number) { this.router.navigate(['/hr/handle-ticket', id]); }
  transferTicket(id: number) {}
  bulkAssign() {} bulkChangeStatus() {} bulkDelete() {}
  previousPage() { if (this.currentPage > 1) this.currentPage--; }
  nextPage() { if (this.currentPage < this.totalPages) this.currentPage++; }
  goToPage(page: number) { this.currentPage = page; }
  showNotifications() {} showSettings() {} toggleUserMenu() {}
  onLogout() { localStorage.removeItem('user'); this.router.navigate(['/auth']); }
}