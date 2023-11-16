import { Column, ColumnDataType } from'@sct-shared-lib';
import { UNIT_TYPE } from '@sct-shared-lib';
import { ControlType } from '@sct-shared-lib';
export interface ImportListRespone {
  importPoint: any;
  countryOfOrigin: any;
  date: Date;
  fuelType: any;
  volume: number;
}

export interface ImportFilter {
  startDate: Date;
  endDate: Date;
  refinery:string;
  fuelType: string;
  countryOfOrigin: string;
  importPoint: string;
}

export const IMPORT_LIST: ImportListRespone[] = [
  {
    importPoint: {
      station: 'KIC Refinery ',
      location: 'Star, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    importPoint: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 440000,
  },
  {
    importPoint: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    importPoint: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    importPoint: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    importPoint: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
    },
    countryOfOrigin: {name:'Malaysia',value:'Malaysia'},
    date: new Date(),
    fuelType: {id:'Gasoline', value:'Gasoline'},
    volume: 4000,
  },
];

export const IMPORT_COLUMNS: Column[] = [
  { name: 'importPoint', displayName: 'Import Point', displayType: ColumnDataType.STRING},
  { name: 'countryOfOrigin', displayName: 'Country of Origin', displayType: ColumnDataType.STRING },
  { name: 'date', displayName: 'Date', displayType: ColumnDataType.STRING },
  { name: 'fuelType', displayName: 'Fuel Type', displayType: ColumnDataType.STRING },
  { name: 'volume', displayName: 'Volume', displayType: ColumnDataType.STRING },
  { name: 'actions', displayName: 'Actions', displayType: ColumnDataType.STRING },

];

export const IMPORT_FILTER = {
  REFINERY_STATE: { name: 'refinery', displayName: 'Refinery', controlType: ControlType.SELECT },
  START_DATE: { name: 'startDate', displayName: 'Start Date', controlType: ControlType.DATETIME },
  END_DATE: { name: 'endDate', displayName: 'EndDate', controlType: ControlType.DATETIME },
  IMPORT_POINT: { name: 'importPoint', displayName: 'Import Point',controlType: ControlType.SELECT },
  COUNTRY_OF_ORIGIN: { name: 'countryOfOrigin', displayName: 'Country of Origin',controlType: ControlType.SELECT },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type',controlType: ControlType.SELECT },
};
//
export const GIM_REFINERY = [
  { name: 'klang', value: 'GIM Refinery Klang, Selangor' },
  { name: 'kanagawa', value: 'GIM Refinery Kanagawa, Penang' },
];
// data for import filter

export const COUNTRY_OF_ORIGIN: any = [
  { name: 'ThaiLand', value: 'ThaiLand' },
  { name: 'Malaysia', value: 'Malaysia' },
];
export const FUEL_TYPE: any = [
  { name: 'Gasoline', value: 'Gasoline' },
  { name: 'Diesel', value: 'Diesel' },
];
export const IMPORT_POINT: any = [
  { name: 'kic', value: 'KIC Refinery' },
  { name: 'mz', value: 'MZ Oil & Gas' },
];


// DETAIL-LIST OF IMPORT
export const IMPORT_FORM = {
  DATE: { name: 'date', displayName: 'Date', controlType: ControlType.DATETIME },
  IMPORT_POINT: { name: 'importPoint', displayName: 'Name of Import Point',controlType: ControlType.INPUTGROUP , iconClass :'bi bi-geo-alt'},
  FUEL_VOLUME: { name: 'fuelVolume', displayName: 'Fuel Volume',controlType: ControlType.INPUTGROUP ,iconContent : UNIT_TYPE.LITRE.displayName },
  COUNTRY_OF_ORIGIN: { name: 'countryOfOrigin', displayName: 'Country of Origin',controlType: ControlType.SELECT },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type',controlType: ControlType.SELECT },
};
//