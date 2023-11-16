import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { forkJoin, map, Observable } from 'rxjs';

import { CHART_DATA_RESPONSE, CHART_NAME, CIRCLE_PROGRESS, LEVEL_PROGRESS } from '../../models';
import { DashboardService } from '../../services/dashboard.service';

import { CHART_CONTROL, CHART_DATASETS, SCHEDULE_TYPES } from '@sct-shared-lib';
import { DATE_FORMAT } from '@sct-shared-lib';
import { FUEL_TYPES } from '@sct-shared-lib';
import { UtilitiesService } from '@sct-shared-lib';
import { LEVEL_PROGRESS_DATASETS } from '@sct-shared-lib';
import { CIRCLE_PROGRESS_DATASETS } from '@sct-shared-lib';

@Component({
  selector: 'sct-dashboard-content',
  templateUrl: './dashboard-content.component.html',
  styleUrls: ['./dashboard-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardContentComponent implements OnInit {
  CHART_NAME = CHART_NAME;
  listChart$!: Observable<CHART_CONTROL[]>;

  importChart$!: Observable<CHART_CONTROL>;
  exportChart$!: Observable<CHART_CONTROL>;

  levelProgressData$!: Observable<LEVEL_PROGRESS_DATASETS[]>
  circleProgressData$!: Observable<CIRCLE_PROGRESS_DATASETS[]>

  scheduleTypes = [...Object.values(SCHEDULE_TYPES)];

  constructor(private _dashboardService: DashboardService, private _utilitiesService: UtilitiesService, private _fb: FormBuilder) {}

  ngOnInit(): void {
    this._getImportDeclaration(SCHEDULE_TYPES.MONTHLY.name);
    this._getExportDeclaration(SCHEDULE_TYPES.MONTHLY.name);
    this._getLevelProgressData();
    this._getCircleProgressData();
  }

  onScheduleChangeImport(scheduleType: string) {
    this._getImportDeclaration(scheduleType);
  }

  onScheduleChangeExport(scheduleType: string) {
    this._getExportDeclaration(scheduleType);
  }

  private _getImportDeclaration(scheduleType: string) {
    const rawData$ = this._dashboardService.getImportDeclaration(scheduleType);
    this.importChart$ = this._formatDataForChart(rawData$, CHART_NAME.IMPORT_DECLARATION, scheduleType);  }

  private _getExportDeclaration(scheduleType: string) {
    const rawData$ = this._dashboardService.getImportDeclaration(scheduleType);
    this.exportChart$ = this._formatDataForChart(rawData$, CHART_NAME.EXPORT_DECLARATION, scheduleType);
  }

  private _getLevelProgressData() {
    const rawData$ = forkJoin([this._dashboardService.getGasolineLevel(), this._dashboardService.getDieselLevel()]).pipe(
      map(([gasolineLevelData, dieselLevelData]) => {
        return {
          gasolineLevelData: gasolineLevelData,
          dieselLevelData:dieselLevelData,
        };
      })
    );

    this.levelProgressData$ = rawData$.pipe(
      map(({ gasolineLevelData, dieselLevelData }) => {
        const gasolineLevel = {...gasolineLevelData[0], progressName: LEVEL_PROGRESS.GASOLINE.progressName};
        const dieselLevel = {...gasolineLevelData[0], progressName: LEVEL_PROGRESS.DIESEL.progressName};
        return [gasolineLevel, dieselLevel]
      })
    );
  }

  private _getCircleProgressData() {
    const rawData$ = forkJoin([this._dashboardService.getGasolineInventory(), this._dashboardService.getDieselInventory()]).pipe(
      map(([gasiolineData, dieselData]) => {
        return {
          gasiolineData: gasiolineData,
          dieselData:dieselData,
        };
      })
    );

    this.circleProgressData$ = rawData$.pipe(
      map(({ gasiolineData, dieselData }) => {
        const gasolineInventory = {...gasiolineData[0], progressName: CIRCLE_PROGRESS.GASOLINE.progressName, icon:  CIRCLE_PROGRESS.GASOLINE.icon, color: CIRCLE_PROGRESS.GASOLINE.color};
        const dieselInventory = {...dieselData[0], progressName: CIRCLE_PROGRESS.DIESEL.progressName, icon:  CIRCLE_PROGRESS.DIESEL.icon, color:CIRCLE_PROGRESS.DIESEL.color };
        return [gasolineInventory, dieselInventory]
      })
    );
  }

  private _formatDataForChart(rawData$: Observable<CHART_DATA_RESPONSE[]>, chartInfo: any, scheduleType: string) {
    return rawData$.pipe(
      map((res) => {
        const result$ = {
          chartName: { name: chartInfo.name, displayName: chartInfo.displayName },
          config: {
            labels: this._getLabelsAnDatasets(res, scheduleType).listLabels,
            datasets: [this._getLabelsAnDatasets(res, scheduleType).gasoline, this._getLabelsAnDatasets(res, scheduleType).diesel],
          },
        };
        return result$;
      })
    );
  }

  private _getLabelsAnDatasets(data: CHART_DATA_RESPONSE[], scheduleType: string) {
    let listLabels: string[] = [];
    let gasoline: CHART_DATASETS = { label: FUEL_TYPES.GASOLINE.display, data: [] };
    let diesel: CHART_DATASETS = { label: FUEL_TYPES.DIESEL.display, data: [] };

    data.forEach((res: CHART_DATA_RESPONSE) => {
      listLabels.push(...this._formatLabelsByDateTime(res.date, scheduleType));
      gasoline.data.push(res.gasoline);
      diesel.data.push(res.diesel);
    });

    return { listLabels, gasoline, diesel };
  }

  private _formatLabelsByDateTime(date: Date, scheduleType: string) {
    let dataFormatted = [];
    if (scheduleType === SCHEDULE_TYPES.DAILY.name) {
      dataFormatted.push(this._utilitiesService.formatDateTime(date, DATE_FORMAT.DATE_MONTH));
    } else if (scheduleType === SCHEDULE_TYPES.MONTHLY.name) {
      dataFormatted.push(this._utilitiesService.formatDateTime(date, DATE_FORMAT.SHORT_MONTH_NAME));
    } else if (scheduleType === SCHEDULE_TYPES.QUARTERLY.name) {
      dataFormatted.push(this._utilitiesService.formatDateTime(date, DATE_FORMAT.QUARTERLY));
    } else {
      dataFormatted.push(this._utilitiesService.formatDateTime(date, DATE_FORMAT.YEAR));
    }
    return dataFormatted;
  }
}