import { Guid } from "guid-typescript";

export interface MovieWithGenreDto {
    id: Guid,
    title: string,
    genres: string[]
}
