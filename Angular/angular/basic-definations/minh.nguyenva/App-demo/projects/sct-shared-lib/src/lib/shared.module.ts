import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { SctTableTemplate, TableComponent } from './components/table/table.component';
import { CommonMessageComponent } from './components/common-message/common-message.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { NgbPaginationModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { DatePickerComponent } from './components/form/date-picker/date-picker.component';
import { SelectComponent } from './components/form/select/select.component';
import { FilterComponent } from './components/filter/filter.component';
import { SimpleMapComponent } from './components/map/simple-map/simple-map.component';
import { MapComponent } from './components/map/map/map.component';
import { ValidatedFieldComponent } from './components/validated-field/validated-field.component';

import { NgbdSortableHeader } from './directives';

import { DynamicPipe ,UnitPipe} from './pipes';

import { CircleProgressComponent } from './components/chart/circle-progress/circle-progress.component';
import { RoundProgressModule } from 'angular-svg-round-progressbar';
import { BarChartComponent } from './components/chart/bar-chart/bar-chart.component';
import { LevelProgressComponent } from './components/chart/level-progress/level-progress.component';
import { SwitchComponent } from './components/form/switch/switch.component';
import { BreadcrumbComponent } from './components/breadscrumb/breadcrumb.component';
import { ModalComponent } from './components/form/modal/modal.component';
import { LoaderComponent } from './components/loader/loader.component';


const MODULES = [NgbPaginationModule, NgbDatepickerModule, RoundProgressModule, ReactiveFormsModule, RouterModule];
const COMPONENTS = [
  TableComponent,
  PaginationComponent,
  DatePickerComponent,
  SelectComponent,
  FilterComponent,
  SimpleMapComponent,
  MapComponent,
  CommonMessageComponent,
  ValidatedFieldComponent,
  BarChartComponent,
  CircleProgressComponent,
  LevelProgressComponent,
  SwitchComponent,
  BreadcrumbComponent,
  ModalComponent,
  LoaderComponent,
];

const DIRECTIVES = [NgbdSortableHeader, SctTableTemplate];
const PIPES = [DynamicPipe, UnitPipe];

@NgModule({
  declarations: [...COMPONENTS, ...DIRECTIVES, ...PIPES],
  imports: [CommonModule, FormsModule, ...MODULES],
  exports: [...COMPONENTS, ...DIRECTIVES, ...PIPES, ...MODULES],
})
export class SharedModule {}
