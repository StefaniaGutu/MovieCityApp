import { Guid } from "guid-typescript";

export interface ReviewDto {
    id: Guid,
    reviewText: string,
    rating: number,
    date: Date,
    username: string,
    userImage: string,
    hasAvailableImage: boolean
}
