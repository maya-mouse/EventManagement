export interface CreateEvent {
    title: string;
    description: string;
    dateTime: Date; 
    location: string;
    capacity: number | null;
    isPublic: boolean;
}
