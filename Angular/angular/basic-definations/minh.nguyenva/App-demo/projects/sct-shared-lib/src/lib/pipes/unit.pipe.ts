import { Pipe, PipeTransform } from '@angular/core';
import { UNIT_TYPE } from './../constant-variable';

@Pipe({
  name: 'unit',
})
export class UnitPipe implements PipeTransform {
  transform(value: number, extention = UNIT_TYPE.LITRE.shortDisplayName): any {
    return `${value} ${extention}`;
  }
}
