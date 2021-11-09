export interface RoverPosition {
    aggregateId: string
    dateTimeOffset: string;
    sequenceNumber: number;
    facingDirection: number;
    latitude: number;
    longitude: number;
    isBlocked: boolean;
    startId: string;
    facingDirectionName: string;
}