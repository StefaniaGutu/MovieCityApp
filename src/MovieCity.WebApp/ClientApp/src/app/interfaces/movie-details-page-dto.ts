import { Guid } from "guid-typescript";
import { ActorWithImageDto } from "./actor-with-image-dto";
import { ReviewDto } from "./review-dto";

export interface MovieDetailsPageDto {
    id: Guid,
    title: string,
    description: string,
    duration: number,
    releaseDate: Date,
    genres: string[],
    actors: ActorWithImageDto[],
    image: string,
    hasAvailableImage: boolean,
    isLiked: boolean,
    isDisliked: boolean,
    likesNo: number,
    dislikesNo: number,
    isInWatchlist: boolean,
    isInWatched: boolean,
    reviews: ReviewDto[]
}
