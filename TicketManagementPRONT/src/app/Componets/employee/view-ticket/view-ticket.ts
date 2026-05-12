import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket } from '../../../Services/ticket';
@Component({
  selector: 'app-view-ticket',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './view-ticket.html',
  styleUrl: './view-ticket.css',
})
export class ViewTicket implements OnInit {
  ticketId: number = 0;
  status: string = '';
  statusClass: string = '';
  ticketSubject: string = '';
  createdDate: string = '';
  createdTime: string = '';
  category: string = '';
  urgency: string = '';
  urgencyClass: string = '';
  originalMessage: string = '';
  lastUpdate: string = '';
  userInitials: string = '';
  userName: string = '';
  handler: string = '';
  handlerInitials: string = '';
  canClose: boolean = false;

  // SLA — בינתיים ערכי ברירת מחדל עד שנחבר לבאק
  slaPercentage: number = 0;
  slaStatus: string = 'sla-ok';
  slaRemaining: string = 'לא ידוע';
  slaTarget: string = 'לא ידוע';

  attachments: { id: number; name: string; size: string }[] = [];
  timeline: { type: string; title: string; time: string; description?: string }[] = [];
  responses: { authorInitials: string; authorName: string; authorRole: string; date: string; time: string; message: string }[] = [];

  commentForm = new FormGroup({
    comment: new FormControl('', { validators: Validators.required, nonNullable: true })
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ticketService: Ticket
  ) {}

  ngOnInit(): void {
    // שליפת פרטי המשתמש מ-localStorage
    const raw = localStorage.getItem('user');
    if (raw) {
      const user = JSON.parse(raw);
      this.userName = `${user.firstName} ${user.lastName}`;
      this.userInitials = `${user.firstName?.charAt(0) ?? ''}${user.lastName?.charAt(0) ?? ''}`;
    }

    // שליפת ה-ID מה-URL
    this.ticketId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTicket();
  }

  loadTicket(): void {
    this.ticketService.getTicketById(this.ticketId).subscribe({
      next: (ticket) => {
        this.ticketSubject = ticket.subject || 'ללא נושא';
        this.originalMessage = ticket.description;
        this.status = ticket.currentStatus;
        this.statusClass = this.getStatusClass(ticket.currentStatus);
        this.urgency = this.getUrgencyLabel(ticket.urgencyLevel);
        this.urgencyClass = this.getUrgencyClass(ticket.urgencyLevel);
        this.category = ticket.category?.categoryName || 'לא ידוע';
        this.createdDate = ticket.openedDate;
        this.createdTime = ticket.openedTime;
        this.canClose = ticket.currentStatus !== 'סגור';

        // מטפל
        if (ticket.ticketAssignments?.length > 0) {
          const assignment = ticket.ticketAssignments[0];
          this.handler = `${assignment.user?.firstName ?? ''} ${assignment.user?.lastName ?? ''}`;
          this.handlerInitials = `${assignment.user?.firstName?.charAt(0) ?? ''}${assignment.user?.lastName?.charAt(0) ?? ''}`;
        }

        // קבצים מצורפים
        this.attachments = (ticket.attachments ?? []).map((a: any) => ({
          id: a.id,
          name: a.fileName,
          size: `${(a.fileSize / 1024).toFixed(2)} KB`
        }));

        // היסטוריית סטטוסים → timeline
        this.timeline = (ticket.ticketStatusHistories ?? []).map((h: any) => ({
          type: 'status',
          title: `סטטוס שונה ל: ${h.newStatus}`,
          time: h.changedAt,
          description: h.comment
        }));

        // הוספת אירוע פתיחה בתחילת ה-timeline
        this.timeline.unshift({
          type: 'created',
          title: 'הפנייה נפתחה',
          time: `${ticket.openedDate} ${ticket.openedTime}`
        });

        // תגובות מהמסג'ים
        this.responses = (ticket.messages ?? []).map((m: any) => ({
          authorInitials: `${m.user?.firstName?.charAt(0) ?? ''}${m.user?.lastName?.charAt(0) ?? ''}`,
          authorName: `${m.user?.firstName ?? ''} ${m.user?.lastName ?? ''}`,
          authorRole: m.user?.jobTitle || 'עובד',
          date: m.sentAt?.split('T')[0] ?? '',
          time: m.sentAt?.split('T')[1]?.substring(0, 5) ?? '',
          message: m.content
        }));

        // עדכון אחרון
        this.lastUpdate = ticket.ticketStatusHistories?.length > 0
          ? ticket.ticketStatusHistories[ticket.ticketStatusHistories.length - 1].changedAt
          : `${ticket.openedDate}`;
      },
      error: () => this.router.navigate(['/employee'])
    });
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

  submitComment(): void {
    if (this.commentForm.invalid) return;
    // כאן נוסיף שליחת תגובה לבאק בהמשך
    console.log('תגובה:', this.commentForm.value.comment);
    this.commentForm.reset();
  }

  goBack(): void { this.router.navigate(['/employee']); }
  attachFile(): void {}
  printTicket(): void { window.print(); }
  shareTicket(): void {}
  closeTicket(): void {
    // כאן נוסיף סגירת פנייה בהמשך
    console.log('סגירת פנייה', this.ticketId);
  }
  downloadFile(fileId: number): void {}
}