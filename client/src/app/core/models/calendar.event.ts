import { Tag } from "./tag";

export interface CalendarEvent {
    id: number
    title: string
    dateTime: Date
    isOrganizer: boolean
    tags: Tag[];
}