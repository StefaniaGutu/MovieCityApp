import { Guid } from "guid-typescript";
import { FriendStatusTypes } from "../enums/friend-status-types";
import { MovieWithGenreDto } from "./movie-with-genre-dto";
import { MovieWithRatingDto } from "./movie-with-rating-dto";

export interface UserProfileDto {
    id: Guid,
    username: string,
    firstName: string,
    fullName: string,
    lastName: string,
    email: string,
    birthDate: Date,
    status: FriendStatusTypes,
    //newImage
    actualImage: string,
    hasAvailableImage: boolean,
    favoriteGenres: string[],
    recentLikedMovies: MovieWithGenreDto[],
    recentReviews: MovieWithRatingDto[],
    hasMoreThan3Reviews: boolean
}
