export enum ControlType {
  TEXTBOX = 'textbox',
  SELECT = 'select',
  DATETIME = 'dateTime',
  INPUTGROUP = 'inputGroup',
}

export interface formControlBase<T> {
  value?: T | undefined;
  key: string;
  label?: string;
  validate?: any[];
  order?: number;
  controlType?: ControlType;
  type?: string;
  iconClass?: string;
  iconContent?: string;
  options: { key: string; value: string; display: string }[];
}

export interface filterInfo {
  name: string;
  displayName: string;
  controlType?: ControlType;
  iconClass?: string;
  iconContent?: string;
}

export interface options {
  id: string;
  name: string;
}
