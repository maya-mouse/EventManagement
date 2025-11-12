import { Component, OnInit, inject } from '@angular/core';
import { Observable, BehaviorSubject, of, catchError, tap, map, combineLatest } from 'rxjs'; // Додано combineLatest
import { Router } from '@angular/router';
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { EventService } from '../../../../core/services/event.service';
import { AuthService } from '../../../../core/services/auth.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Tag } from '../../../../core/models/tag';
import { Event } from '../../../../core/models/event';

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


 public availableTags$: Observable<Tag[]> = this.eventService.getAvailableTags();
 public selectedTagNames: string[] = []; 
 searchControl = new FormControl(''); 

 ngOnInit(): void {
 this.isLoggedIn$.subscribe(status => this.isLoggedIn = status);

 this.loadEvents();


 this.searchControl.valueChanges.pipe(
 debounceTime(300),
 distinctUntilChanged(),
 tap(term => this.applyCombinedFilter(term, this.selectedTagNames))
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
 map((events: Event[]) => events.filter(event => event.id > 0))
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
 
 this.eventsSubject.next(this.allEvents); // Initial display
 });
 }


 applyCombinedFilter(searchTerm: string | null, tagNames: string[]): void {
 const term = (searchTerm || '').toLowerCase();
 let filtered = this.allEvents;


 if (term) {
 filtered = filtered.filter(event => 
 event.title.toLowerCase().includes(term) ||
 event.description.toLowerCase().includes(term) ||
 event.location.toLowerCase().includes(term)
 );
 }
    
    if (tagNames.length > 0) {
        filtered = filtered.filter(event => {

            return event.tags.some(tag => tagNames.includes(tag.name));
        });
    }

 this.eventsSubject.next(filtered);
 }
  

  onTagSelect(tag: Tag, event: any): void {
      if (event.target.checked) {
          this.selectedTagNames = [...this.selectedTagNames, tag.name];
      } else {
          this.selectedTagNames = this.selectedTagNames.filter(t => t !== tag.name);
      }
      this.applyCombinedFilter(this.searchControl.value, this.selectedTagNames);
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