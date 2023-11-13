import { UNIT_TYPE } from '@sct-shared-lib';
import { ControlType } from '@sct-shared-lib';

export const CONFIGURATION_FORM = {
    GASOLINE_THRESHOLD: { name: 'gasoline_threshold', displayName: 'National Gasoline Fuel Threshold', controlType: ControlType.TEXTBOX },
    GASOLINE_UNIT: { name: 'gasoline_unit', displayName: '', controlType: ControlType.SELECT },
    DIESEL_THRESHOLD: { name: 'diesel_threhold', displayName: 'National Diesel Fuel Threshold', controlType: ControlType.TEXTBOX },
    DIESEL_UNIT: { name: 'diesel_unit', displayName: '', controlType: ControlType.SELECT },
  };
  

  export const CONFIGURATION_UNIT: any = [
    { name: UNIT_TYPE.LITRE.name, value: UNIT_TYPE.LITRE.shortDisplayName },
    { name: UNIT_TYPE.CUBE_METRE.name, value: UNIT_TYPE.CUBE_METRE.shortDisplayName },

  ];