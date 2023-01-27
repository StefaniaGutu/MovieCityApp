import { Guid } from "guid-typescript";
import { GenreDto } from "./genre-dto";

export interface MovieWithDetailsDto {
    id: Guid,
    title: string,
    description: string,
    genres: GenreDto[],
    image: string,
    isSeries: boolean,
    hasAvailableImage: boolean
}
