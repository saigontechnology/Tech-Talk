import { UNIT_TYPE } from '@sct-shared-lib';
import { ControlType } from '@sct-shared-lib';

export const SWICHES = [
  { name: 'deliveryStatus', displayName: 'Delivery Status', detail: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' },
  { name: 'emailNotification', displayName: 'Email Notification', detail: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' },
  { name: 'deliveryStatus1', displayName: 'Delivery Status1', detail: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' },
  { name: 'deliveryStatus2', displayName: 'Delivery Status2', detail: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' },
  { name: 'deliveryStatus3', displayName: 'Delivery Status3', detail: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' },
];

export const SETTINGS_COUNTRY: any = [
  { name: 'thaiLand', value: 'ThaiLand' },
  { name: 'malaysia', value: 'Malaysia' },
];

export const SETTING_FORM = {
  DATE: { name: 'date', displayName: 'Date', controlType: ControlType.DATETIME },
  IMPORT_POINT: { name: 'importPoint', displayName: 'Name of Import Point',controlType: ControlType.INPUTGROUP , iconClass :'bi bi-geo-alt'},
  FUEL_VOLUME: { name: 'fuelVolume', displayName: 'Fuel Volume',controlType: ControlType.INPUTGROUP ,iconContent : UNIT_TYPE.LITRE.displayName },
  COUNTRY: { name: 'country', displayName: 'Country',controlType: ControlType.SELECT },
  FUEL_TYPE: { name: 'fuelType', displayName: 'Fuel Type',controlType: ControlType.SELECT },
};