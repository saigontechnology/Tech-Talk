import { Injectable } from '@angular/core';
import { of, timer } from 'rxjs';
import { delay, debounceTime, throttleTime, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ValueService {
  value = 'real value';

  getValue() { return this.value; }
  setValue(value: string) { this.value = value; }

  getObservableValue() { return of('observable value'); }
  getObservableValue2() { return of('observable value 2'); }

  getPromiseValue() { 
    return Promise.resolve('promise value'); 
  }

  getObservableDelayValue() {
    return of('observable delay value')
            .pipe(
              tap(()=>{setTimeout(()=>{},4000)})
            );
  }
}
