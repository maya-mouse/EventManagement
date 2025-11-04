import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { Observable, BehaviorSubject, of, catchError, tap, map, filter, take } from 'rxjs';
import { EventService } from '../../../../core/services/event.service';
import { AuthService } from '../../../../core/services/auth.service';
import { CalendarEvent } from '../../../../core/models/calendar.event';
import { CalendarDay } from '../../../../core/models/calendar-day';

@Component({
    selector: 'app-my-events-calendar',
    templateUrl: './my-events-calendar.html',
    standalone: true,
    imports: [CommonModule, AsyncPipe, DatePipe, RouterLink]
})
export class MyEventsCalendarComponent implements OnInit {

    private eventService = inject(EventService);
    private authService = inject(AuthService);
    public router = inject(Router);

    public isLoading: boolean = true;

    private eventsSubject = new BehaviorSubject<CalendarEvent[]>([]);
    public events$: Observable<CalendarEvent[]> = this.eventsSubject.asObservable();

    public view: 'month' | 'week' = 'month';
    public viewDate: Date = new Date();

    public daysInView: CalendarDay[] = [];
    public weekDays: CalendarDay[] = [];

    public showOnlyOrganized: boolean = false;

    public viewModel$: Observable<{ events: CalendarEvent[], hasEvents: boolean }> = this.eventsSubject.asObservable().pipe(
        map(events => ({
            events: events,
            hasEvents: events.length > 0,
        }))
    );

    ngOnInit(): void {
        this.authService.isLoggedIn$.pipe(
            filter(isLoggedIn => isLoggedIn),
            take(1)
        ).subscribe(() => {
            this.loadEvents();
        });
    }

    loadEvents(): void {
        this.isLoading = true;

        this.eventService.getUserCalendarEvents()
            .pipe(
                tap(() => this.isLoading = false),
                catchError(error => {
                    this.isLoading = false;
                    return of([]);
                })
            )
            .subscribe((events: CalendarEvent[]) => {
                this.eventsSubject.next(events);
                this.updateCalendarView(events);
            });
    }

    private updateCalendarView(allEvents: CalendarEvent[]): void {

    
        const filteredEvents = this.showOnlyOrganized
            ? allEvents.filter(e => e.isOrganizer)
            : allEvents;

        if (this.view === 'month') {
            this.generateCalendarDays(this.viewDate);
            this.distributeEventsToDays(this.daysInView, filteredEvents);
        } else { 
            this.generateWeekDays(this.viewDate, filteredEvents);
        }
    }

    private distributeEventsToDays(days: CalendarDay[], filteredEvents: CalendarEvent[]): void {

        days.forEach(day => day.events = []);

        filteredEvents.forEach(event => {
            const eventDate = new Date(event.dateTime);
            const targetDay = days.find(day => day.date.toDateString() === eventDate.toDateString());

            if (targetDay) {
                targetDay.events.push(event);
            }
        });
    }


    private generateCalendarDays(date: Date): void {
        this.daysInView = [];
        const startOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const startDayOfWeek = startOfMonth.getDay();
        const daysFromPrevMonth = (startDayOfWeek === 0 ? 6 : startDayOfWeek - 1);

        const totalCells = 42;

        for (let i = 0; i < totalCells; i++) {
            const day = new Date(startOfMonth);
            day.setDate(startOfMonth.getDate() - daysFromPrevMonth + i);
            day.setHours(0, 0, 0, 0);

            this.daysInView.push({
                date: day,
                dayOfMonth: day.getDate(),
                isCurrentMonth: day.getMonth() === date.getMonth(),
                isToday: day.toDateString() === today.toDateString(),
                events: []
            });
        }
    }

    private generateWeekDays(date: Date, allEvents: CalendarEvent[]): void {
        this.weekDays = [];

        const dayOfWeek = date.getDay();
        const daysToSubtract = (dayOfWeek === 0 ? 6 : dayOfWeek - 1);

        const startOfWeek = new Date(date);
        startOfWeek.setDate(date.getDate() - daysToSubtract);
        startOfWeek.setHours(0, 0, 0, 0);

        for (let i = 0; i < 7; i++) {
            const currentDay = new Date(startOfWeek);
            currentDay.setDate(startOfWeek.getDate() + i);
            currentDay.setHours(0, 0, 0, 0);

            const dayEvents = allEvents.filter(event =>
                new Date(event.dateTime).toDateString() === currentDay.toDateString()
            );

            const today = new Date();
            today.setHours(0, 0, 0, 0);

            this.weekDays.push({
                date: currentDay,
                dayOfMonth: currentDay.getDate(),
                isCurrentMonth: currentDay.getMonth() === date.getMonth(),
                isToday: currentDay.toDateString() === today.toDateString(),
                events: dayEvents
            });
        }
    }


    setView(newView: 'month' | 'week'): void {
        this.view = newView;
        this.updateCalendarView(this.eventsSubject.getValue());
    }

    toggleOrganizedFilter(): void {
        this.showOnlyOrganized = !this.showOnlyOrganized;
        this.updateCalendarView(this.eventsSubject.getValue());
    }

    private changeViewDate(amount: number): void {
        if (this.view === 'month') {
            const currentMonth = this.viewDate.getMonth();
            const currentYear = this.viewDate.getFullYear();
            this.viewDate = new Date(currentYear, currentMonth + amount, 1);
        } else {
            this.viewDate.setDate(this.viewDate.getDate() + (amount * 7));
        }

        this.updateCalendarView(this.eventsSubject.getValue());
    }

    prev(): void {
        this.changeViewDate(-1);
    }

    next(): void {
        this.changeViewDate(1);
    }

    onEventClick(eventId: number): void {
        this.router.navigate(['/events', eventId]);
    }

    onDayClick(day: CalendarDay): void {
    }
}