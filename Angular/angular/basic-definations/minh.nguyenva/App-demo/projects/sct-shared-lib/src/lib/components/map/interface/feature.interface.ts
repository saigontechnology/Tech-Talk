import { Coordinates } from "../site";

export interface FeatureStyle {
    locationType: string | any;
    isSelected?: boolean;
}

export interface MapEvent {
    id: any;
    coordinates: Coordinates;
}