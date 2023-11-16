export interface Country {
  id: string;
  name: string;
  countryCode: string;
  coordinates: LocationCoordinates;
}

export interface CountryData {
  loaded : boolean;
  content : Country;
}

export interface LocationCoordinates{
  x: number;
  y: number;
}

export interface LocationSiteResponse {
    id: string;
    name:string;
    internalCode:string;
    coordinates : LocationCoordinates;
    typeName: string; 
    typeCode: string;
    country: Country;
    
}

export interface LocationSite{
  loaded : boolean;
  content : LocationSiteResponse;
}