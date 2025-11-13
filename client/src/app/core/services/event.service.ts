import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event } from '../models/event';
import { environment } from '../../../environments/environment';
import { EventDetail } from '../models/event.detail';
import { CalendarEvent } from '../models/calendar.event';
import { CreateEvent } from '../models/create.event';
import { Tag } from '../models/tag';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private apiUrl = `${environment.apiUrl}/Events`;
  private http = inject(HttpClient);

  getPublicEvents(searchTerm: string = '', tagNames: string[] = []): Observable<Event[]> {
    let params = new HttpParams();
    
    if (searchTerm) {
        params = params.set('SearchTerm', searchTerm);
    }
    
    tagNames.forEach(tag => {
        params = params.append('TagNames', tag);
    });
    
    const res = this.http.get<Event[]>(this.apiUrl, { params });

    return res;
  }
  updateEvent(id: number, updateEvent: CreateEvent): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}`, updateEvent);
  }
  createEvent(newEvent: CreateEvent): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.apiUrl, newEvent);
  }

  getEventDetails(eventId: number): Observable<EventDetail> {

    const res = this.http.get<EventDetail>(`${this.apiUrl}/${eventId}`);
    return res;
  }
  getUserCalendarEvents(): Observable<CalendarEvent[]> {
    const res = this.http.get<CalendarEvent[]>(`${this.apiUrl}/me/events`);
    return res;
  }

  deleteEvent(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }


  joinEvent(eventId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${eventId}/join`, {});
  }


  leaveEvent(eventId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${eventId}/leave`, {});
  }

  getAvailableTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${environment.apiUrl}/Tags`); 
  }
}
