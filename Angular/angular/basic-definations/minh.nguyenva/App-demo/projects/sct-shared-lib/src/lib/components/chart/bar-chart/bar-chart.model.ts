export const colorChart = [
  {name: 'secondary', color : '#CFD1D3'},
  {name: 'primary', color : '#0087c8'}
]

export interface CHART_DATASETS {
  borderWidth?: number;
  barThickness?: number;
  borderRadius?: number;
  backgroundColor?: string;
  data: number[];
  label: string;
}
export interface CHART_NAME {
  name: string; displayName: string
}
export interface CHART_CONTROL {
  chartName: CHART_NAME,
  config: CHART_CONFIG;
}

export interface CHART_CONFIG {
  labels: string [];
  datasets : CHART_DATASETS[];
}

export const SCHEDULE_TYPES = {
  DAILY: { name: 'daily', label: 'Daily' },
  MONTHLY: { name: 'monthly', label: 'Monthly' },
  QUARTERLY: { name: 'quarterly', label: 'Quarterly' },
  YEARLY: { name: 'yearly', label: 'Yearly' },
};