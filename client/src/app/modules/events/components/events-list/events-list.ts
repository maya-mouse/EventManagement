import { Component, OnInit, inject } from '@angular/core';
import { Observable, BehaviorSubject, of, catchError, tap, map } from 'rxjs';
import { Router } from '@angular/router';
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { EventService } from '../../../../core/services/event.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Event } from '../../../../core/models/event'; 
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators'; 

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.html',
  standalone: true,
  imports: [CommonModule, AsyncPipe, DatePipe, ReactiveFormsModule]
})
export class EventsListComponent implements OnInit {

  private eventService = inject(EventService);
  private authService = inject(AuthService);
  public router = inject(Router);


  private allEvents: Event[] = []; 
  private eventsSubject = new BehaviorSubject<Event[]>([]);
  public events$: Observable<Event[]> = this.eventsSubject.asObservable();

  public isLoading: boolean = true;
  public isLoggedIn$: Observable<boolean> = this.authService.isLoggedIn$;
  public isLoggedIn: boolean = false;


  searchControl = new FormControl('');

  ngOnInit(): void {
    this.isLoggedIn$.subscribe(status => this.isLoggedIn = status);
    this.loadEvents();
    

    this.searchControl.valueChanges.pipe(
        debounceTime(300), 
        distinctUntilChanged(),
        tap(term => this.applyFilter(term)) 
    ).subscribe();
  }

  loadEvents(): void {
    this.isLoading = true;


    this.eventService.getPublicEvents() 
      .pipe(
        tap(() => this.isLoading = false),
        catchError(error => {
          this.isLoading = false;
          return of([]);
        }),
      )
      .subscribe((events: Event[]) => {

        this.allEvents = events.map(event => {
          const capacity = event.capacity ?? 0;
          const participantsCount = event.participantsCount ?? 0;
          
          return {
            ...event,
            isJoined: event.isJoined ?? false,
            isFull: capacity > 0 && participantsCount >= capacity,
          } as Event;
        });
        

        this.eventsSubject.next(this.allEvents); 
      });
  }

  applyFilter(searchTerm: string | null): void {
    const term = (searchTerm || '').toLowerCase();
    
    if (!term) {
        this.eventsSubject.next(this.allEvents); 
        return;
    }

    const filtered = this.allEvents.filter(event => 
        event.title.toLowerCase().includes(term) ||
        event.description.toLowerCase().includes(term) ||
        event.location.toLowerCase().includes(term)
    );
    
    this.eventsSubject.next(filtered); 
  }


  onJoin(event: Event): void {
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }
    this.eventService.joinEvent(event.id).subscribe({
      next: () => this.updateEventParticipation(event.id, true),
      error: (err) => console.error('Failed to join event:', err)
    });
  }

  onLeave(event: Event): void {
    if (!this.isLoggedIn) return;
    this.eventService.leaveEvent(event.id).subscribe({
      next: () => this.updateEventParticipation(event.id, false),
      error: (err) => console.error('Failed to leave event:', err)
    });
  }

  onCardClick(eventId: number, event: MouseEvent): void {
    if ((event.target as HTMLElement).closest('button, span')) {
      return;
    }
    this.onViewDetails(eventId);
  }

  private updateEventParticipation(eventId: number, joined: boolean): void {
    
    const updateArray = (arr: Event[]): Event[] => {
        return arr.map(e => {
            if (e.id === eventId) {
                const newCount = joined ? e.participantsCount + 1 : e.participantsCount - 1;
                return {
                    ...e,
                    isJoined: joined,
                    participantsCount: newCount,
                    isFull: e.capacity !== null && newCount >= e.capacity
                } as Event;
            }
            return e;
        });
    };
    
  
    this.allEvents = updateArray(this.allEvents);
    this.eventsSubject.next(updateArray(this.eventsSubject.getValue())); 
  }

  onViewDetails(eventId: number): void {
    this.router.navigate(['/events', eventId]);
  }
}