import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-hr-home',
  imports: [CommonModule, FormsModule],
  templateUrl: './hr-home.html',
  styleUrl: './hr-home.css',
})
export class HrHome {
// משתמש
userInitials = 'ח.י';
userName = 'חנה יעקבוביץ';
notificationCount = 0;

// תצוגה
view: 'queue' | 'kanban' = 'queue';
filterStatus = 'all';
searchQuery = '';
sortBy = 'date';

// סטטיסטיקות
assignedCount = 0;
urgentCount = 0;
pendingCount = 0;
pendingProgress = 0;
resolvedTodayCount = 0;
avgResponseTime = '0h';
satisfactionScore = 0;
satisfactionStars = 0;
newCount = 0;
inProgressCount = 0;
totalCount = 0;

// בחירה מרובה
selectAll = false;

// פאג'ינציה
currentPage = 1;
totalPages = 1;
totalItems = 0;
pages: number[] = [];
showNotifications() {}
showSettings() {}
toggleUserMenu() {}
onLogout() {}

setView(view: 'queue' | 'kanban') {
  this.view = view;
}

setFilter(status: string) {
  this.filterStatus = status;
}

refreshTickets() {}

toggleSelectAll() {}

selectedCount() {
  return 0;
}

handleTicket(id: number) {}
viewDetails(id: number) {}
transferTicket(id: number) {}

bulkAssign() {}
bulkChangeStatus() {}
bulkDelete() {}

previousPage() {}
nextPage() {}
goToPage(page: number) {}
pageSize = 10;

get startItem(): number {
  return this.totalItems === 0
    ? 0
    : (this.currentPage - 1) * this.pageSize + 1;
}

get endItem(): number {
  return Math.min(this.currentPage * this.pageSize, this.totalItems);
}
tickets = [
  {
    id: 1,
    subject: 'בקשת חופשה',
    employee: 'ישראל ישראלי',
    employeeName: 'ישראל ישראלי',
    employeeInitials: 'יי',
    department: 'פיתוח',
    urgent: true,
    selected: false,
    urgency: 'high',
    urgencyClass: 'urgent-high',
    urgencyText: 'דחוף',
    category: 'חופשה',
    categoryClass: 'cat-vacation',
    aiConfidence: 92,
    preview: 'בקשה לחופשה של 3 ימים...',
    riskLevel: 'risk-low',
    riskScore: 2,
    riskLabel: 'נמוך',
    slaStatus: 'sla-ok',
    slaRemaining: '4 שעות',
    slaPercentage: 75,
    status: 'פתוח',
    statusClass: 'status-open'
  }
];

hasSelectedTickets(): boolean {
  return this.tickets.some(t => t.selected);
}

}
