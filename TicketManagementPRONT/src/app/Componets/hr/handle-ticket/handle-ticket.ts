import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket } from '../../../Services/ticket';


@Component({
  selector: 'app-handle-ticket',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './handle-ticket.html',
  styleUrl: './handle-ticket.css',
})
export class HandleTicket implements OnInit {
  ticketId = 0;
  currentUserId = 0;
  userInitials = '';
  userName = '';

  // פרטי הפנייה
  ticketSubject = '';
  status = '';
  statusClass = '';
  urgency = '';
  urgencyClass = '';
  createdDate = '';
  ticketMessage = '';
  attachments: { id: number; name: string }[] = [];

  // פרטי עובד
  employeeName = '';
  employeeInitials = '';
  employeeDept = '';
  employeeId = '';

  // AI
  aiCategory = '';
  aiCategoryConfidence = 0;
  aiUrgencyText = '';
  aiUrgencyScore = 0;
  aiUrgencyClass = '';
  aiRiskScore = 0;
  aiRiskDescription = '';
  aiSentiment = 'neutral';
  aiSentimentClass = 'neutral';
  aiSentimentText = 'ניטרלי';
  aiKeywords: string[] = [];
  suggestedResponse = '';
  aiConfidence = 0;

  // SLA
  slaTarget = 'לא ידוע';
  slaRemaining = 'לא ידוע';
  slaWarning = false;

  responseForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private ticketService: Ticket
  ) {}

  ngOnInit(): void {
    const raw = localStorage.getItem('user');
    if (raw) {
      const user = JSON.parse(raw);
      this.currentUserId = user.id;
      this.userInitials = `${user.firstName?.charAt(0) ?? ''}${user.lastName?.charAt(0) ?? ''}`;
      this.userName = `${user.firstName} ${user.lastName}`;
    }

    this.responseForm = this.fb.group({
      response: ['', Validators.required],
      newStatus: ['בטיפול']
    });

    this.ticketId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTicket();
  }

  loadTicket(): void {
    this.ticketService.getTicketById(this.ticketId).subscribe({
      next: (ticket) => {
        this.ticketSubject = ticket.subject || 'ללא נושא';
        this.status = ticket.currentStatus;
        this.statusClass = this.getStatusClass(ticket.currentStatus);
        this.urgency = this.getUrgencyLabel(ticket.urgencyLevel);
        this.urgencyClass = this.getUrgencyClass(ticket.urgencyLevel);
        this.createdDate = ticket.openedDate;
        this.ticketMessage = ticket.description;

        // עובד
        this.employeeName = `${ticket.openedByUser?.firstName ?? ''} ${ticket.openedByUser?.lastName ?? ''}`;
        this.employeeInitials = `${ticket.openedByUser?.firstName?.charAt(0) ?? ''}${ticket.openedByUser?.lastName?.charAt(0) ?? ''}`;
        this.employeeDept = ticket.openedByUser?.jobTitle ?? '';
        this.employeeId = ticket.openedByUser?.employeeId ?? '';

        // קבצים
        this.attachments = (ticket.attachments ?? []).map((a: any) => ({
          id: a.id,
          name: a.fileName
        }));

        // SLA
        const sla = ticket.category?.slas?.find(
          (s: any) => s.urgencyLevel === ticket.urgencyLevel
        );
        if (sla) {
          this.slaTarget = `${sla.targetResponseHours} שעות`;
          const openedAt = new Date(`${ticket.openedDate}T${ticket.openedTime}`);
          const now = new Date();
          const hoursElapsed = (now.getTime() - openedAt.getTime()) / (1000 * 60 * 60);
          const hoursRemaining = sla.targetResponseHours - hoursElapsed;
          if (hoursRemaining > 0) {
            this.slaRemaining = `${Math.round(hoursRemaining)} שעות`;
            this.slaWarning = hoursRemaining < sla.targetResponseHours * 0.2;
          } else {
            this.slaRemaining = 'חריגה!';
            this.slaWarning = true;
          }
        }

        // AI
        const ai = ticket.aianalyses?.[0];
        if (ai) {
          this.aiCategory = ticket.category?.categoryName ?? 'לא ידוע';
          this.aiCategoryConfidence = 90;
          this.aiConfidence = 90;
          this.aiUrgencyScore = Number(ai.urgencyScore);
          this.aiUrgencyText = this.getUrgencyLabel(Math.round(this.aiUrgencyScore));
          this.aiUrgencyClass = this.getUrgencyClass(Math.round(this.aiUrgencyScore));
          this.aiRiskScore = this.getRiskScore(ai.riskLevel);
          this.aiRiskDescription = ai.riskLevel ?? '';
          this.suggestedResponse = ai.analysisText ?? '';
          this.aiKeywords = ai.analysisText?.split(' ').slice(0, 5) ?? [];

          // טון רגשי לפי רמת סיכון
          if (ai.riskLevel === 'High') {
            this.aiSentiment = 'negative';
            this.aiSentimentClass = 'negative';
            this.aiSentimentText = 'שלילי';
          } else if (ai.riskLevel === 'Low') {
            this.aiSentiment = 'positive';
            this.aiSentimentClass = 'positive';
            this.aiSentimentText = 'חיובי';
          } else {
            this.aiSentiment = 'neutral';
            this.aiSentimentClass = 'neutral';
            this.aiSentimentText = 'ניטרלי';
          }
        }
      },
      error: (err) => {
        console.log('error:', err);
        this.router.navigate(['/hr']);
      }
    });
  }

  sendResponse(): void {
    if (this.responseForm.invalid) return;

    const content = this.responseForm.value.response;
    const newStatus = this.responseForm.value.newStatus;

    this.ticketService.addMessage(this.ticketId, {
      userId: this.currentUserId,
      content
    }).subscribe();

    this.ticketService.updateStatus(this.ticketId, {
      newStatus,
      userId: this.currentUserId,
      comment: content
    }).subscribe({
      next: () => this.router.navigate(['/hr'])
    });
  }

  useSuggestion(): void {
    this.responseForm.patchValue({ response: this.suggestedResponse });
  }

  editSuggestion(): void {
    this.responseForm.patchValue({ response: this.suggestedResponse });
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

  getRiskScore(risk: string): number {
    switch (risk) {
      case 'Low': return 3; case 'Medium': return 6; case 'High': return 9;
      default: return 0;
    }
  }

  goBack(): void { this.router.navigate(['/hr']); }
  downloadFile(id: number): void {}
  transferTicket(): void {}
  escalate(): void {}
  addNote(): void {}
  closeTicket(): void {
    this.ticketService.updateStatus(this.ticketId, {
      newStatus: 'סגור',
      userId: this.currentUserId,
      comment: 'פנייה נסגרה'
    }).subscribe({
      next: () => this.router.navigate(['/hr'])
    });
  }
}