import { Guid } from "guid-typescript";

export interface ActorDetailsPageDto {
    id: Guid,
    name: string,
    description: string,
    image: string,
    hasAvailableImage: boolean
}
