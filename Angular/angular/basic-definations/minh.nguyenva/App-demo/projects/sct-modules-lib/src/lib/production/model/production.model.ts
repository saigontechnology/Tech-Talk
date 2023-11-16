import { Column, ColumnDataType, DestinationSite, UNIT_TYPE, ControlType, FuelTypesReponse, Volume, SortType } from '@sct-shared-lib';


export interface ProductionListRequest {
  siteId: string;
  requestedDate: string;
  fuelTypeId: string;
  volume: Volume;
}

export interface ProductionListResponse {
  id: string;
  requestedDate: string;
  fuelType: FuelTypesReponse;
  volume: Volume;
}

export interface ProductionFilterValue {
  startDate?: any;
  endDate?: any;
  fuelType?: string;
}

export interface ProductionPayLoad {
  siteId?: string;
  startDate?: Date;
  endDate?: Date;
  fuelType?: string;
}

export const PRODUCTION_COLUMNS: Column[] = [
  { name: 'date', displayName: 'Date', displayType: ColumnDataType.STRING, sort: SortType.EMPTY },
  { name: 'fuelType', displayName: 'Fuel Type', displayType: ColumnDataType.STRING, sort: SortType.EMPTY },
  { name: 'volume', displayName: 'Volume', displayType: ColumnDataType.STRING, sort: SortType.EMPTY },
  { name: 'actions', displayName: 'Actions', displayType: ColumnDataType.STRING, sort: SortType.EMPTY },
];

export const PRODUCTION_FILTER = {
  LOCATION_SITE: { name: 'siteId', displayName: 'Location Site', controlType: ControlType.SELECT },
  START_DATE: { name: 'startDate', displayName: 'Start Date', controlType: ControlType.DATETIME },
  END_DATE: { name: 'endDate', displayName: 'EndDate', controlType: ControlType.DATETIME },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type', controlType: ControlType.SELECT },
};
//

// data for import filter
export const PRODUCTION_FORM = {
  DATE: { name: 'date', displayName: 'Date', controlType: ControlType.DATETIME },
  PRODUCTION_POINT: { name: 'productionPoint', displayName: 'Name of Production Point', controlType: ControlType.SELECT },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type', controlType: ControlType.SELECT },
  FUEL_VOLUME: {
    name: 'fuelVolume',
    displayName: 'Fuel Volume',
    controlType: ControlType.INPUTGROUP,
    iconContent: UNIT_TYPE.LITRE.displayName,
  },
};
