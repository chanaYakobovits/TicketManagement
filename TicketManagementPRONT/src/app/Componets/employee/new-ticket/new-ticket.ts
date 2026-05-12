import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Ticket } from '../../../Services/ticket';
import { Router } from '@angular/router';
@Component({
  selector: 'app-new-ticket',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './new-ticket.html',
  styleUrl: './new-ticket.css',
})
export class NewTicket {
  userInitials: string = 'HN';
  userName: string = 'חנה';
  ticketForm: FormGroup;
  uploadedFiles: File[] = [];
  isSubmitting = false;
  submitSuccess = false;
  submitError = '';

  constructor(private fb: FormBuilder, private ticketService: Ticket,private router: Router) {
    this.ticketForm = this.fb.group({
      subject: [''],
      message: ['', [Validators.required, Validators.minLength(10)]]
    });

    const userData = localStorage.getItem('user');
    if (userData) {
      const user = JSON.parse(userData);
      this.currentUserId = user.id;
      this.userName = user.firstName + ' ' + user.lastName;
      this.userInitials = (user.firstName?.charAt(0) ?? '') + (user.lastName?.charAt(0) ?? '');
    }
  }
  currentUserId: number = 0;
  onSubmit() {
    if (this.ticketForm.invalid) return;

    this.isSubmitting = true;
    this.submitError = '';


    const payload = {
      subject: this.ticketForm.value.subject || '',
      description: this.ticketForm.value.message,
      openedByUserId: this.currentUserId
    };

    this.ticketService.addTicket(payload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        this.submitSuccess = true;
        this.ticketForm.reset();
        this.uploadedFiles = [];
        this.router.navigate(['/employee']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.submitError = 'אירעה שגיאה בשליחת הפנייה. נסה שנית.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const control = this.ticketForm.get(fieldName);
    return !!(control && control.invalid && (control.touched || control.dirty));
  }

goBack() {
  this.router.navigate(['/employee']);
}

onCancel() {
  this.ticketForm.reset();
  this.uploadedFiles = [];
  this.router.navigate(['/employee']);
}

  getCharCount() {
    return this.ticketForm.get('message')?.value.length || 0;
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDragLeave(event: DragEvent) { }

  onDrop(event: DragEvent) {
    event.preventDefault();
    const files = event.dataTransfer?.files;
    if (files) this.uploadedFiles.push(...Array.from(files));
  }

  onFileSelect(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files) this.uploadedFiles.push(...Array.from(target.files));
  }

  removeFile(index: number) {
    this.uploadedFiles.splice(index, 1);
  }

  formatFileSize(bytes: number) {
    return `${(bytes / 1024).toFixed(2)} KB`;
  }
}
