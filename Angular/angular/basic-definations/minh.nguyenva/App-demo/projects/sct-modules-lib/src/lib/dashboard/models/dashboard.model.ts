import { UNIT_TYPE } from '@sct-shared-lib';

export const CHART_NAME = {
  IMPORT_DECLARATION: { name: 'importDeclaration', displayName: 'Import Declaration' },
  EXPORT_DECLARATION: { name: 'exportDeclaration', displayName: 'Export Declaration' },
};

export const LEVEL_PROGRESS = {
  GASOLINE: { name: 'gasolineLevel', progressName: 'National Gasoline Level' },
  DIESEL: { name: 'dieselLevel', progressName: 'National Diesel Level' },
};

export const CIRCLE_PROGRESS = {
  GASOLINE: { name: 'gasoline', progressName: 'Gasoline' ,icon :'../../../../assets/images/icon/gasoline.png',color: '#0087C8'},
  DIESEL: { name: 'diesel', progressName: 'Diesel' , icon :'../../../../assets/images/icon/diesel.png',color: '#F48A00' },
};

export interface CHART_DATA_RESPONSE {
  date: Date;
  gasoline: number;
  diesel: number;
}

export interface LEVEL_PROGRESS_RESPONSE {
  unitType: string;
  currentVolume: number;
  maxVolume: number;
  percentVolume: number;
}

export interface INVENTORY_PROGRESS_RESPONSE {
  currentVolume: number;
  percentVolume: number;
}

// data
export const IMPORT_DECLARE_DATA_DAILY: CHART_DATA_RESPONSE[] = [
  {
    date: new Date('2022-11-27'),
    gasoline: 300,
    diesel: 401,
  },
  {
    date: new Date('2022-11-28'),
    gasoline: 122,
    diesel: 535,
  },
  {
    date: new Date('2022-11-29'),
    gasoline: 1224,
    diesel: 620,
  },
  {
    date: new Date('2022-11-30'),
    gasoline: 435,
    diesel: 735,
  },
  {
    date: new Date('2022-12-01'),
    gasoline: 893,
    diesel: 903,
  },
  {
    date: new Date('2022-12-02'),
    gasoline: 253,
    diesel: 830,
  },
  {
    date: new Date('2022-12-03'),
    gasoline: 30,
    diesel: 70,
  },
];
export const IMPORT_DECLARE_DATA_MONTHLY: CHART_DATA_RESPONSE[] = [
  {
    date: new Date('2022-06-01'),
    gasoline: 302,
    diesel: 140,
  },
  {
    date: new Date('2022-07-01'),
    gasoline: 122,
    diesel: 155,
  },
  {
    date: new Date('2022-08-01'),
    gasoline: 124,
    diesel: 190,
  },
  {
    date: new Date('2022-09-01'),
    gasoline: 100,
    diesel: 375,
  },
  {
    date: new Date('2022-10-01'),
    gasoline: 200,
    diesel: 390,
  },
  {
    date: new Date('2022-11-01'),
    gasoline: 95,
    diesel: 222,
  },
  {
    date: new Date('2022-12-01'),
    gasoline: 30,
    diesel: 90,
  },
];
export const IMPORT_DECLARE_DATA_QUATERLY: CHART_DATA_RESPONSE[] = [
  {
    date: new Date('2022-01-01'),
    gasoline: 3025,
    diesel: 1402,
  },
  {
    date: new Date('2022-04-01'),
    gasoline: 1224,
    diesel: 1552,
  },
  {
    date: new Date('2022-07-01'),
    gasoline: 1242,
    diesel: 1920,
  },
  {
    date: new Date('2022-10-01'),
    gasoline: 1243,
    diesel: 1290,
  },

];
export const IMPORT_DECLARE_DATA_YEARLY: CHART_DATA_RESPONSE[] = [
  {
    date: new Date('2018-01-01'),
    gasoline: 341,
    diesel: 422,
  },
  {
    date: new Date('2019-01-01'),
    gasoline: 452,
    diesel: 555,
  },
  {
    date: new Date('2020-01-01'),
    gasoline: 908,
    diesel: 444,
  },
  {
    date: new Date('2021-01-01'),
    gasoline: 780,
    diesel: 459,
  },
  {
    date: new Date('2022-01-01'),
    gasoline: 460,
    diesel: 663,
  },
];

export const GASOLINE_LEVEL: LEVEL_PROGRESS_RESPONSE[] = [
  { currentVolume: 50000, maxVolume: 100000, percentVolume: 50, unitType: UNIT_TYPE.CUBE_METRE.shortDisplayName },
];

export const GASOLINE_INVENTORY: INVENTORY_PROGRESS_RESPONSE[] = [{ currentVolume: 50000, percentVolume: 50 }];
