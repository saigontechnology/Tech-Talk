import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  Input,
  EventEmitter,
  Output,
  ViewChild,
  OnChanges,
  SimpleChanges,
  ElementRef,
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { ChartConfiguration } from 'chart.js';
import Chart from 'chart.js/auto';

import { CHART_CONFIG, colorChart, SCHEDULE_TYPES } from './bar-chart.model';

@Component({
  selector: 'sct-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BarChartComponent implements OnInit, OnChanges {
  @Input() chartConfig: CHART_CONFIG = {
    labels: [],
    datasets: [],
  };

  @Input() chartName: { name: string; displayName: string } = {
    name: '',
    displayName: '',
  };

  @Output() scheduleChange = new EventEmitter<any>();
  @ViewChild('barChart', { static: true }) barChart: ElementRef = {} as ElementRef;
  public chart!: any;

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      title: {
        display: true,
      },
      legend: {
        display: false,
      },
    },
    scales: {
      x: {
        grid: {
          display: false,
        },
      },
      y: {
        grid: {},
        ticks: {
          callback(tickValue) {
            return tickValue.toString() + 'K';
          },
        },
      },
    },
  };

  scheduleTypes = [...Object.values(SCHEDULE_TYPES)];
  form!: FormGroup;
  chartLegend: any[] = [];

  constructor(private _fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    this._initChartConfig();
    this._updateChart();
  }

  ngOnInit(): void {
    this._initForm();
    this._initialChart();
  }

  onClickLegend(event : Event , datasetIndex: number){
   const isHidden = !this.chart.isDatasetVisible(datasetIndex);
   this.chart.setDatasetVisibility(datasetIndex,isHidden);
   this.chart.update();
  }

  private _initLegend() {
    this.chart.legend.legendItems.forEach((dataset: any, index: number) => {
      console.log(dataset)
      const text = dataset.text;
      const datasetIndex = dataset.datasetIndex;
      const bgColor = dataset.fillStyle;
      const bColor = dataset.strokeStyle;
      const fontColor = dataset.fontColor;

      const legenData = { text, datasetIndex, bgColor, bColor, fontColor };
      this.chartLegend.push(legenData);
    });
  }

  private _initialChart() {
    this.chart = new Chart(this.barChart.nativeElement, {
      type: 'bar',
      data: this.chartConfig,
      options: this.barChartOptions,
    });
    this._initLegend();
  }

  private _updateChart() {
    if (this.chart) {
      this.chart.data = this.chartConfig;
      this.chart.update();
    }
  }

  private _initChartConfig() {
    this.chartConfig.datasets.forEach((res, idx) => {
      res.backgroundColor = colorChart[idx].color;
      res.borderRadius = Number.MAX_VALUE;
      res.barThickness = 15;
      res.borderWidth = 0;
    });
    this.chart?.chart?.update();
  }

  private _initForm() {
    if (this.chartName) {
      const group: any = {};
      group[this.chartName.name] = SCHEDULE_TYPES.MONTHLY.name;
      this.form = this._fb.group({
        ...group,
      });
      this.form.valueChanges.subscribe((formValue) => this.scheduleChange.emit(formValue[this.chartName.name]));
    }
  }
}
