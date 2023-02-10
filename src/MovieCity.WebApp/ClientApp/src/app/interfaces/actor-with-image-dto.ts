import { Guid } from "guid-typescript";

export interface ActorWithImageDto {
    id: Guid,
    name: string,
    image: string,
    hasAvailableImage: boolean
}
