import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-handle-ticket',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './handle-ticket.html',
  styleUrl: './handle-ticket.css',
})
export class HandleTicket {

  constructor(private fb: FormBuilder) {
    this.responseForm = this.fb.group({
      response: ['']
    });
  }

  // ===== Ticket Info =====
  ticketId = 3421;
  createdDate = '18/02/2026';

  status = 'פתוח';
  statusClass = 'open';

  urgency = 'גבוה';
  urgencyClass = 'high';

  slaTarget = '24 שעות';
  slaRemaining = '3 שעות';
  slaWarning = true;

  // ===== AI =====
  aiRiskScore = 8;
  aiRiskDescription = 'סיכון גבוה לחריגה';

  aiSentiment = 'neutral';
  aiSentimentClass = 'neutral';
  aiSentimentText = 'ניטרלי';

  aiConfidence = 87;
  aiCategory = 'שכר';
  aiCategoryConfidence = 90;

  aiUrgencyText = 'גבוה';
  aiUrgencyScore = 8;
  aiUrgencyClass = 'high';

  aiKeywords: string[] = ['שכר', 'תשלום', 'איחור'];

  suggestedResponse = 'נבדוק את הנושא ונעדכן בהקדם.';

  responseForm!: FormGroup;

  attachments = [
    { id: 1, name: 'salary.pdf' }
  ];

  // ===== Actions =====
  goBack() {}

  downloadFile(id: number) {}

  useSuggestion() {
    this.responseForm.patchValue({
      response: this.suggestedResponse
    });
  }

  editSuggestion() {}

  sendResponse() {}

  transferTicket() {}

  escalate() {}

  addNote() {}

  closeTicket() {}
}
