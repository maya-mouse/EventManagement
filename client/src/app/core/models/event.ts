import { Tag } from "./tag"

export interface Event {
    id: number
    title: string
    description: string
    dateTime: Date
    location: string
    capacity: number | null
    participantsCount: number
    isFull: boolean
    isJoined: boolean
    tags: Tag[]
}