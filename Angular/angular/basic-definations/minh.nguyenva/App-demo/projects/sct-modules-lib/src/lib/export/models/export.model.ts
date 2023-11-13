import { Column, ColumnDataType } from'@sct-shared-lib';
import { UNIT_TYPE } from '@sct-shared-lib';
import { ControlType } from '@sct-shared-lib';
export interface ExportListRespone {
  id: string;
  pointOfOrigin: any;
  exportCountry: string;
  date: Date;
  fuelType: any;
  volume: number;
}

export interface ExportFilter {
  startDate: Date;
  endDate: Date;
  refinery:string;
  fuelType: string;
  country: string;
  pointOfOrigin: string;
}

export const EXPORT_FILTER = {
  REFINERY_STATE: { name: 'refinery', displayName: 'Refinery', controlType: ControlType.SELECT },
  START_DATE: { name: 'startDate', displayName: 'Start Date', controlType: ControlType.DATETIME },
  END_DATE: { name: 'endDate', displayName: 'EndDate', controlType: ControlType.DATETIME },
  COUNTRY: { name: 'country', displayName: 'Country',controlType: ControlType.SELECT },
  POINT_OF_ORIGIN: { name: 'pointOfOrigin', displayName: 'Point of Origin',controlType: ControlType.SELECT },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type',controlType: ControlType.SELECT },
};


export const EXPORT_LIST: ExportListRespone[] = [
  {
    id:'123',
    pointOfOrigin: {
      station: 'KIC Refinery ',
      location: 'Star, NYC',
      country: 'malaysia',
    },
    exportCountry: 'USA',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id:'1223',
    pointOfOrigin: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
      country: 'malaysia',
    },
    exportCountry: 'Russia',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 440000,
  },
  {
    id:'789',
    pointOfOrigin: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
      country: 'malaysia',
    },
    exportCountry: 'Russia',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id:'331',
    pointOfOrigin: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
      country: 'malaysia',
    },
    exportCountry: 'Russia',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id:'352',
    pointOfOrigin: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
      country: 'malaysia',
    },
    exportCountry: 'Russia',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id:'12443',
    pointOfOrigin: {
      station: 'KIC Refinery',
      location: 'Bangsar, Kualumpur',
      country: 'malaysia',
    },
    exportCountry: 'Russia',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
];

export const EXPORT_COLUMNS: Column[] = [
  { name: 'pointOfOrigin', displayName: 'Point Of Orgin', displayType: ColumnDataType.STRING },
  { name: 'exportCountry', displayName: 'Export Country', displayType: ColumnDataType.STRING },
  { name: 'date', displayName: 'Date', displayType: ColumnDataType.STRING },
  { name: 'fuelType', displayName: 'Fuel Type', displayType: ColumnDataType.STRING },
  { name: 'volume', displayName: 'Volume', displayType: ColumnDataType.STRING },
  { name: 'actions', displayName: 'Actions', displayType: ColumnDataType.STRING },
];

export const EXPORT_COUNTRY_OF_ORIGIN: any = [
  { name: 'thaiLand', value: 'ThaiLand' },
  { name: 'malaysia', value: 'Malaysia' },
];

export const EXPORT_COUNTRY: any = [
  { name: 'kualalumpur', value: 'Kualalunpur' },
  { name: 'selangor', value: 'Selengor' },
];
export const EXPORT_FUEL_TYPE: any = [
  { name: 'gasoline', value: 'Gasoline' },
  { name: 'diesel', value: 'Diesel' },
];
export const EXPORT_POINT_OF_ORIGIN: any = [
  { name: 'kic', value: 'KIC Refinery' },
  { name: 'mz', value: 'MZ Oil & Gas' },
];

export const EXPORT_GIM_REFINERY = [
  { name: 'klang', value: 'GIM Refinery Klang, Selangor' },
  { name: 'kanagawa', value: 'GIM Refinery Kanagawa, Penang' },
]; 
// DETAIL-LIST OF EXPORT
export const EXPORT_FORM = {
  DATE: { name: 'date', displayName: 'Date', controlType: ControlType.DATETIME },
  POINT_OF_ORIGIN: { name: 'pointOfOrigin', displayName: 'Point of Origin',controlType: ControlType.SELECT },
  EXPORT_COUNTRY: { name: 'exportCountry', displayName: 'Export Country',controlType: ControlType.INPUTGROUP , iconClass :'bi bi-geo-alt' },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type',controlType: ControlType.SELECT },
  FUEL_VOLUME: { name: 'fuelVolume', displayName: 'Fuel Volume',controlType: ControlType.INPUTGROUP ,iconContent : UNIT_TYPE.LITRE.displayName },
};