export interface Event {
    id: number
    title: string
    Description: string
    dateTime: Date
    location: string
    capacity: number | null
    participantsCount: number
    isJoined: boolean
    isFull: boolean
}