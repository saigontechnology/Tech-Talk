import { Params } from '@angular/router';
import { Observable } from 'rxjs';

export interface Breadcrumb {
  displayName$: Observable<string>;
  params?: Params;
  path?: string;
}
