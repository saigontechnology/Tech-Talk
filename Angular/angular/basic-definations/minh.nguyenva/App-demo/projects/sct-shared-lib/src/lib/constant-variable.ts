export enum APPLICATION_ROLES {
  VIEW_IMPORTS = 'VIEW_IMPORTS',
  VIEW_INLANDS = 'VIEW_INLANDS',
  VIEW_PRODUCTIONS = 'VIEW_PRODUCTIONS',
  VIEW_SALES = 'VIEW_SALES',
  VIEW_EXPORTS = 'VIEW_EXPORTS',
  VIEW_REPORTS = 'VIEW_REPORTS',
  VIEW_CONFIGURATION = 'VIEW_CONFIGURATION',
  VIEW_SETTINGS = 'VIEW_SETTINGS',
  VIEW_DASHBOARD = 'VIEW_DASHBOARD',
}
export interface SiteType {
  typeName: string;
  typeCode: string;
  list: string[];
}

export const SITE_TYPE = {
  REFINERIES: { typeCode:'REFINERIES' ,typeName: 'Refineries', lists: [...APPLICATION_ROLES.VIEW_EXPORTS, ...APPLICATION_ROLES.VIEW_PRODUCTIONS] },
  INLAND_DEPOT: { typeCode: 'INLAND_DEPOT',typeName: 'INLAND_DEPOT', lists: [...APPLICATION_ROLES.VIEW_INLANDS] },
  IMPORT_DEPOT: { typeCode:'IMPORT_DEPOT',typeName: 'IMPORT_DEPOT', lists: [...APPLICATION_ROLES.VIEW_INLANDS, ...APPLICATION_ROLES.VIEW_IMPORTS] },
  GAS_STATION: { typeCode:'GAS_STATION',typeName: 'GAS_STATION', lists: [...APPLICATION_ROLES.VIEW_SALES] },
};

export const TABLE_NAME = {
  IMPORT: 'List Of Import',
  EXPORT: 'List Of Export',
  PRODUCTION: 'List Of Production',
  FUEL_SALES: 'List Of Fuel Sales',
  DASHBOARD_IMPORT: 'Import Declaration',
  DASHBOARD_EXPORT: 'Export Declaration',
};

export const PATH = {
  DASHBOARD: { displayName: 'Dashboard', route: 'dashboard' },

  FUEL_SALES: { displayName: 'List of Fuel Sale', route: 'fuel-sales' },
  FUEL_SALES_UPDATE: { displayName: 'Update Fuel Sales', route: 'fuel-sales-update' },
  FUEL_SALES_CREATE: { displayName: 'Declare New Fuel Sales', route: 'fuel-sales-create' },

  IMPORT: { displayName: 'List Of Import', route: 'import' },
  IMPORT_UPDATE: { displayName: 'Update Import List', route: 'import-update' },
  IMPORT_CREATE: { displayName: 'Declare New Import', route: 'import-create' },

  EXPORT: { displayName: 'List Of Export', route: 'export' },
  EXPORT_UPDATE: { displayName: 'Update Export List', route: 'export-update' },
  EXPORT_CREATE: { displayName: 'Declare New Export', route: 'export-create' },

  PRODUCTION: { displayName: 'List Of Production', route: 'production' },
  PRODUCTION_UPDATE: { displayName: 'Update Production List', route: 'production-update' },
  PRODUCTION_CREATE: { displayName: 'Declare New Production', route: 'prodiction-create' },

  REPORTS: { displayName: 'Reports', route: 'reports' },
  SETTINGS: { displayName: 'Settings', route: 'settings' },
  CONFIGURATION: { displayName: 'Configuration', route: 'configuration' },
};

export const ICONS = {
  IMPORT: 'bi bi-card-list',
  EXPORT: 'bi bi-card-list',
  PRODUCTION: 'bi bi-card-list',
  FUEL_SALES: 'bi bi-card-list',
  LOGOUT: 'bi bi-box-arrow-left',
  REPORT: 'bi bi-file-earmark-bar-graph',
  SETTINGS: 'bi bi-gear',
  DASHBOARD: 'bi bi-layout-wtf',
  CONFIGURATION: 'bi bi-sliders',
};

export const UNIT_TYPE = {
  LITRE: { id: 0, name: 'litres', displayName: 'Litres', shortDisplayName: 'ℓ' },
  CUBE_METRE: { id: 0, name: 'cubeMeter', displayName: 'Cube metre', shortDisplayName: 'm³' },
};

export const MESSAGE = {
  fieldRequired: 'This field is required',
};

// simple-map

export const MARKER_URLS = {
  MARKER_LOCATION: './assets/images/map/markers/marker.png',
  MARKER_LOCATION_SELECTED: './assets/images/map/markers/marker_selected.png',

  MARKER_LOCATION2: './assets/images/map/markers/dark-marker.png',
  MARKER_LOCATION2_SELECTED: './assets/images/map/markers/marker_selected.png',

  MARKER_GAS_STATION: './assets/images/map/markers/yellow-marker.png',
  MARKER_GAS_STATION_SELECTED: './assets/images/map/markers/yellow-marker_selected.png',

  MARKER_TANK_FARM: './assets/images/map/markers/violet-marker.png',
  MARKER_TANK_FARM_SELECTED: './assets/images/map/markers/violet-marker_selected.png',

  MARKER_DEFAULT: './assets/images/map/markers/pink-marker.png',
  MARKER_DEFAULT_SELECTED: './assets/images/map/markers/pink-marker.png',

  MARKER_OTHER: './assets/images/map/markers/green-marker.png',
  MARKER_OTHER_SELECTED: './assets/images/map/markers/green-marker_selected.png',
};

export const LOCATION_TYPE_TO_MARKER_MAP: { [key: string]: string } = {
  ['Gas Station']: 'MARKER_GAS_STATION',
  ['Petrol Station']: 'MARKER_GAS_STATION',
  ['Tank Farm']: 'MARKER_TANK_FARM',
  ['Terminal']: 'MARKER_OTHER',
  ['District']: 'MARKER_OTHER',
  ['Truck']: 'MARKER_OTHER',
  ['Train']: 'MARKER_OTHER',
};

export enum MAP_LAYERS {
  FEATURES = 'FEATURES',
  CONTROL = 'CONTROL',
}

// CHART

export const FUEL_TYPES = {
  GASOLINE: { name: 'gasoline', display: 'Gasoline' },
  DIESEL: { name: 'diesel', display: 'Diesel' },
};

export const CRUD_MODE = {
  CREATE: 'CREATE',
  UPDATE: 'UPDATE',
};
