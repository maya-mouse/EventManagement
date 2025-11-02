import { Component, OnInit } from '@angular/core';
import { Observable, BehaviorSubject, of, catchError, switchMap, tap } from 'rxjs';
import { Event } from '../../../../core/models/event.model';
import { EventService } from '../../../../core/services/event.service';
import { Router } from '@angular/router';
import { NgIf, NgFor, DatePipe, AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.html',
  imports: [NgIf, NgFor, DatePipe, AsyncPipe]
})
export class EventsListComponent implements OnInit {

  private eventsSubject = new BehaviorSubject<Event[]>([]);
  public events$: Observable<Event[]> = this.eventsSubject.asObservable();
  
  public isLoading: boolean = true;
  public isLoggedIn: boolean = false; 

  constructor(
    private eventService: EventService,
    private router: Router
  ) { }

  ngOnInit(): void {
    
    this.loadEvents();
  }

  
  loadEvents(): void {
    this.isLoading = true;
    this.eventService.getPublicEvents()
      .pipe(
        tap(() => this.isLoading = false),
        catchError(error => {
          console.error('Error loading events:', error);
          this.isLoading = false;
          return of([]); 
        })
      )
      .subscribe(events => {
        
        const eventsWithFullStatus = events.map(event => ({
          ...event,
          isFull: event.capacity !== null && event.participantsCount >= event.capacity
        }));
        this.eventsSubject.next(eventsWithFullStatus);
      });
  }


  onViewDetails(eventId: number): void {
    this.router.navigate(['/events', eventId]);
  }
}