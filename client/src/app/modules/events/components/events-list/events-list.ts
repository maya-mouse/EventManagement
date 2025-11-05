import { Component, OnInit, inject } from '@angular/core';
import { Observable, BehaviorSubject, of, catchError, tap, map } from 'rxjs'; // Додано map
import { Router } from '@angular/router';
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { EventService } from '../../../../core/services/event.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Event } from '../../../../core/models/event';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.html',
  standalone: true,
  imports: [CommonModule, AsyncPipe, DatePipe]
})
export class EventsListComponent implements OnInit {

  private eventService = inject(EventService);
  private authService = inject(AuthService);
  public router = inject(Router);

  private eventsSubject = new BehaviorSubject<Event[]>([]);
  public events$: Observable<Event[]> = this.eventsSubject.asObservable();

  public isLoading: boolean = true;

  public isLoggedIn$: Observable<boolean> = this.authService.isLoggedIn$;
  public isLoggedIn: boolean = false;

  ngOnInit(): void {
    this.isLoggedIn$.subscribe(status => this.isLoggedIn = status);
    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;

    this.eventService.getPublicEvents()
      .pipe(
        tap(() => this.isLoading = false),
        catchError(error => {
          console.log(error)
          this.isLoading = false;
          return of([]);
        }),
        map((events: Event[]) => events)
      )
      .subscribe((events: Event[]) => {

        const eventsWithFullStatus = events.map(event => {
          return {
            ...event,
            isJoined: event.isJoined ?? false,
          } as Event;
        });
        this.eventsSubject.next(eventsWithFullStatus);
      });
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

    const currentEvents = this.eventsSubject.getValue();
    const updatedEvents = currentEvents.map(e => {
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
    this.eventsSubject.next(updatedEvents);
  }

  onViewDetails(eventId: number): void {
    this.router.navigate(['/events', eventId]);
  }
}