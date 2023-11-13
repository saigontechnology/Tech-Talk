export interface FuelTypesReponse {
  id: string;
  name: string;
  abbreviation?: string;
  nameI18n?: string;
  desityMax?: number;
  desityMin?: number;
  tolerance?: number;
}

export interface FuelTypes {
  loaded : boolean;
  content : FuelTypesReponse
}