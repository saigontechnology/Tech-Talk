import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SimpleCalculatorService {

  add(n1: number, n2:number) {
    return n1 + n2;
  }

  subtract(n1: number, n2:number) {
    return n1 - n2;
  }
}

