import { CalendarEvent } from "./calendar.event";

export interface CalendarDay {
    date: Date;
    dayOfMonth: number;
    isCurrentMonth: boolean;
    isToday: boolean;
    events: CalendarEvent[];
}