import { MovieWithDetailsDto } from "./movie-with-details-dto";

export interface PopularAndLikedMoviesDto {
    mostPopularMovies: MovieWithDetailsDto[],
    mostLikedMovies: MovieWithDetailsDto[]
}
