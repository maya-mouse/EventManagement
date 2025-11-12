import { ParticipantModel } from "./participant";
import { Tag } from "./tag";

export interface EventDetail {
    id: number;
    title: string;
    description: string;
    dateTime: Date; 
    location: string;
    capacity: number | null;
    isPublic: boolean;
    
    host: ParticipantModel;
    hostId: number; 
    
    participants: ParticipantModel[];
    tags: Tag[];
    isJoined: boolean;
    isOrganizer: boolean; 

    
  
    participantsCount: number;
    isFull: boolean;
}