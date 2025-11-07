import { Component, OnInit, inject, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router'; // Додано Router
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { EventService } from '../../../../core/services/event.service';
import { AuthService } from '../../../../core/services/auth.service';
import { EventDetail } from '../../../../core/models/event.detail';
import { Observable, switchMap, catchError, of, tap, filter, Subscription, BehaviorSubject } from 'rxjs'; // Додано Subscription

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.html',
  standalone: true,
  imports: [CommonModule, DatePipe, AsyncPipe, RouterLink]
})
export class EventDetailsComponent implements OnInit, OnDestroy {

  private route = inject(ActivatedRoute);
  private eventService = inject(EventService);
  private authService = inject(AuthService);
  public router = inject(Router);

  private eventIdSubject = new BehaviorSubject<number | null>(null);
  event$!: Observable<EventDetail | null>;
  private authSubscription!: Subscription;
  public isLoading: boolean = true;
  public errorMessage: string | null = null;
  public isLoggedIn$: Observable<boolean> = this.authService.isLoggedIn$;
  public isLoggedIn: boolean = false;

  ngOnInit(): void {
    this.authSubscription = this.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
    });


    this.event$ = this.route.paramMap.pipe(

      filter(params => params.has('id')),
      tap(params => {
        const eventId = Number(params.get('id'));
        this.eventIdSubject.next(eventId);
      }),

      switchMap(() => this.eventIdSubject),
      filter(id => id !== null),
      switchMap(eventId => {
        this.isLoading = true;
        this.errorMessage = null;

        return this.eventService.getEventDetails(eventId!).pipe(
          tap(() => this.isLoading = false),
          catchError(error => {
            this.isLoading = false;
            this.errorMessage = (error.status === 404) ? 'Event not found.' : 'Data loading error';
            return of(null as EventDetail | null);
          })
        );
      })
    );
  }

  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
  }

  onJoin(event: EventDetail): void {
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }
    this.eventService.joinEvent(event.id)
      .subscribe({
        next: () => {
          this.eventIdSubject.next(event.id);
        }
      });
  }

  onLeave(event: EventDetail): void {
    if (!this.isLoggedIn) return;
    this.eventService.leaveEvent(event.id)
      .subscribe({
        next: () => {
          this.eventIdSubject.next(event.id);
        }

      });
  }

  onDelete(eventId: number): void {
    if (confirm('Are you sure you want to delete this event?')) {

      this.eventService.deleteEvent(eventId)
        .subscribe({
          next: () => {
            this.router.navigate(['/events']);
          },
          error: (err) => {
            console.error('Failed to delete event:', err);
            alert('Error deleting event. Check organizer permissions');
          }
        });
    }
  }
}