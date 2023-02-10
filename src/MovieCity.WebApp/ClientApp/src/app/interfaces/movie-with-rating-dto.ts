import { Guid } from "guid-typescript";

export interface MovieWithRatingDto {
    id: Guid,
    title: string,
    rating: number
}
