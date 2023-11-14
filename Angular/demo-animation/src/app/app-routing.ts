import { Routes } from '@angular/router';
import { OpenCloseComponent } from './demo/open-close/open-close.component';
import { CompareComponent } from './demo/compare/compare.component';

export const routes: Routes = [
  {
    path: '',
    component: OpenCloseComponent,
  },
  {
    path: 'compare',
    component: CompareComponent,
  },
];
