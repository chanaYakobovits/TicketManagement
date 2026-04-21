import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-view-ticket',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './view-ticket.html',
  styleUrl: './view-ticket.css',
})
export class ViewTicket {
  ticketId = 123;
  status = 'Open';
  statusClass = 'status-open';
  ticketSubject = 'נושא טסט';
  createdDate = '01/02/2026';
  createdTime = '14:30';
  category = 'תמיכה';
  urgency = 'High';
  urgencyClass = 'urgency-high';
  originalMessage = 'זהו ההודעה המקורית';

  attachments: { id: number; name: string; size: number }[] = [];
  timeline: { type: string; title: string; time: string; description?: string }[] = [];
  responses: { authorInitials: string; authorName: string; authorRole: string; date: string; time: string; message: string }[] = [];
  uploadedFiles: { id: number; name: string; size: number }[] = [];

  handler = 'יוסי כהן';
  handlerInitials = 'י.כ';
  slaPercentage = 50;
  slaStatus = 'sla-warning';
  slaRemaining = '2h';
  slaTarget = '4h';
  canClose = true;

  ticketForm: FormGroup<{ 
    subject: FormControl<string>; 
    message: FormControl<string>; 
  }> = new FormGroup({
    subject: new FormControl('', { validators: Validators.required, nonNullable: true }),
    message: new FormControl('', { validators: Validators.required, nonNullable: true }),
  });
  
  commentForm: FormGroup<{ 
    comment: FormControl<string>; 
  }> = new FormGroup({
    comment: new FormControl('', { validators: Validators.required, nonNullable: true })
  });
  

  lastUpdate: string = '';
  userInitials = 'ח.י';
  userName = 'חנה יעקבוביץ';

  // פונקציות דמו
  goBack() {}
  onSubmit() {}
  submitComment() {}
  attachFile() {}
  printTicket() {}
  shareTicket() {}
  closeTicket() {}
  downloadFile(fileId: number) {}
  isFieldInvalid(fieldName: string) { return false; }
  getCharCount() { return 20; }
  onDragOver(event: Event) {}
  onDragLeave(event: Event) {}
  onDrop(event: Event) {}
  onFileSelect(event: Event) {}
  formatFileSize(size: number) { return size + ' KB'; }
  removeFile(index: number) {}
  onCancel() {}
}
