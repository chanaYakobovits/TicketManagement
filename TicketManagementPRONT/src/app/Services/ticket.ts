import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from "../env/environment.dev";
import { Observable } from 'rxjs';
import { TicketDTO } from '../models/TicketDTO.model';
@Injectable({
  providedIn: 'root',
})
export class Ticket {
  baseUrl = environment.URLS.apiUrl;
  BASE_URL = this.baseUrl + 'Ticket';

  http: HttpClient = inject(HttpClient);

  constructor() { }
  getTicketsByUser(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.BASE_URL}/user/${userId}`);
  }
getTicketById(id: number): Observable<any> {
  return this.http.get<any>(`${this.BASE_URL}/${id}`);
}

  addTicket(ticket: TicketDTO): Observable<any> {
    return this.http.post(this.BASE_URL, ticket);
  }
}
