import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

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

  constructor(private fb: FormBuilder) {
    this.ticketForm = this.fb.group({
      message: ['', Validators.required]
    });
  }
  isFieldInvalid(fieldName: string): boolean {
    const control = this.ticketForm.get(fieldName);
    return !!(control && control.invalid && (control.touched || control.dirty));
  }
  
  goBack() {
    console.log('Going back');
  }

  onSubmit() {
    console.log('Form submitted', this.ticketForm.value);
  }

  onCancel() {
    console.log('Cancelled');
  }

  getCharCount() {
    return this.ticketForm.get('message')?.value.length || 0;
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDragLeave(event: DragEvent) {}

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
