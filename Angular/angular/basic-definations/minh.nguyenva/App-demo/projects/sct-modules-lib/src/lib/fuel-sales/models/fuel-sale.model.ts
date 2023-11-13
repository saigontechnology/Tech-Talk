import { Column, ColumnDataType } from'@sct-shared-lib';
import { UNIT_TYPE } from '@sct-shared-lib';
import { ControlType } from '@sct-shared-lib';
export interface FuelSalesResponse {
  id: string;
  date: Date;
  fuelType: any;
  volume: number;
}

export interface FuelSalesFilter {
  startDate: Date;
  endDate: Date;
  refinery: string;
  fuelType: string;
}

export const FUEL_SALES_LIST: FuelSalesResponse[] = [
  {
    id: '123',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id: '456',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 440000,
  },
  {
    id: '789',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id: '1512',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id: '321',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
  {
    id: '321',
    date: new Date(),
    fuelType: {id:'gasoline', value:'Gasoline'},
    volume: 4000,
  },
];

export const FUEL_SALES_COLUMNS: Column[] = [
  { name: 'date', displayName: 'Date', displayType: ColumnDataType.STRING },
  { name: 'fuelType', displayName: 'Fuel Type', displayType: ColumnDataType.STRING },
  { name: 'volume', displayName: 'Volume', displayType: ColumnDataType.STRING },
  { name: 'actions', displayName: 'Actions', displayType: ColumnDataType.STRING },
];

export const FUEL_SALES_FILTER = {
  REFINERY_STATE: { name: 'refinery', displayName: 'Refinery', controlType: ControlType.SELECT },
  START_DATE: { name: 'startDate', displayName: 'Start Date', controlType: ControlType.DATETIME },
  END_DATE: { name: 'endDate', displayName: 'EndDate', controlType: ControlType.DATETIME },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type', controlType: ControlType.SELECT },
};
//
export const FUEL_SALES_GIM_REFINERY = [
  { name: 'klang', value: 'GIM Refinery Klang, Selangor' },
  { name: 'kanagawa', value: 'GIM Refinery Kanagawa, Penang' },
];
// data for import filter

export const FUEL_SALES_FUEL_TYPE: any = [
  { name: 'gasoline', value: 'Gasoline' },
  { name: 'diesel', value: 'Diesel' },
];


export const FUEL_SALES_FORM = {
  DATE: { name: 'date', displayName: 'Date', controlType: ControlType.DATETIME },
  TANK_ID: { name: 'tankID', displayName: 'Tank ID', controlType: ControlType.TEXTBOX },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type', controlType: ControlType.SELECT },
  FUEL_VOLUME: {
    name: 'fuelVolume',
    displayName: 'Fuel Volume',
    controlType: ControlType.INPUTGROUP,
    iconContent: UNIT_TYPE.LITRE.displayName,
  },
};
