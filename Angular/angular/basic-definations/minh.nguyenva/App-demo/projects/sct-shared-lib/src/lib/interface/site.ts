import { Country, LocationCoordinates } from './location-site';

export interface GeneralSite {
  id: string;
  name: string;
  internalCode: string;
  coordinates: LocationCoordinates;
  typeName: string;
  typeCode: string;
  country: Country;
}

export interface OriginSite extends GeneralSite {}

export interface DestinationSite extends GeneralSite {}
