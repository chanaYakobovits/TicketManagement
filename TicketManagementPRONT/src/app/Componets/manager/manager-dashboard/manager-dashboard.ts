import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manager-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './manager-dashboard.html',
  styleUrl: './manager-dashboard.css',
})
export class ManagerDashboard {
  

    // ===== Filters =====
    dateRange = 'week';
  
    // ===== User =====
    userInitials = 'ח״י';
    userName = 'חנה יעקבוביץ';
  
    // ===== KPIs =====
    totalTickets = 124;
    newTicketsToday = 7;
  
    slaBreaches = 3;
    slaAtRisk = 5;
  
    resolutionRate = 82;
    resolvedTickets = 98;
  
    avgResponseTime = '2.4 שעות';
  
    // ===== Categories =====
    categories = [
      { name: 'גבוה', percentage: 40, count: 50, class: 'high' },
      { name: 'בינוני', percentage: 35, count: 44, class: 'medium' },
      { name: 'נמוך', percentage: 25, count: 30, class: 'low' }
    ];
    
    // ===== SLA Alerts =====
    slaAlerts = [
      {
        id: 1,
        title: 'חריגת SLA – פנייה #3421',
        employeeName: 'דוד לוי',
        category: 'שכר',
        timeLeft: 'שעה',
        handler: 'שרה כהן'
      },
      {
        id: 2,
        title: 'סיכון SLA – פנייה #3412',
        employeeName: 'מיכל אוחנה',
        category: 'טכני',
        timeLeft: '3 שעות',
        handler: 'דוד לוי'
      }
    ];
    
  
    // ===== Team =====
    teamMembers = [
      {
        name: 'דוד לוי',
        initials: 'ד״ל',
        role: 'HR Specialist',
        openTickets: 5,
        active: 3,
        resolved: 2,
        satisfaction: 88
      },
      {
        name: 'שרה כהן',
        initials: 'ש״כ',
        role: 'HR Manager',
        openTickets: 3,
        active: 1,
        resolved: 2,
        satisfaction: 94
      }
    ];
    
    // ===== AI Insights =====
    aiInsights = [
      {
        title: 'עומס חריג',
        description: 'ריבוי פניות בין 08:00–10:00',
        type: 'warning'
      },
      {
        title: 'שיפור בזמני תגובה',
        description: 'ירידה של 15% בזמן תגובה',
        type: 'success'
      },
      {
        title: 'רוב הפניות טכניות',
        description: '65% מהפניות החודש',
        type: 'info'
      }
    ];
    
    // ===== Actions =====
    exportData() {
      console.log('export data');
    }
  
    viewAlert(id: number) {
      console.log('view alert', id);
    }
    
}
