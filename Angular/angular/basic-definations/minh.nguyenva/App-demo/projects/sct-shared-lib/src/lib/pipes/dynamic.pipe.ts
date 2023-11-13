import { Pipe, PipeTransform } from '@angular/core';
import { CurrencyPipe, DatePipe, DecimalPipe, LowerCasePipe, TitleCasePipe, UpperCasePipe } from '@angular/common';
import { UnitPipe } from './unit.pipe';

@Pipe({
  name: 'dynamicPipe',
})
export class DynamicPipe implements PipeTransform {
  private static readonly PIPE_MAPPINGS: any = {
    number: new DecimalPipe('en-US'),
    lowercase: new LowerCasePipe(),
    titlecase: new TitleCasePipe(),
    currency: new CurrencyPipe('en-US'),
    uppercase: new UpperCasePipe(),
    date: new DatePipe('en-US'),
    unit : new UnitPipe,
  };

  transform(value: any, args: any[]): any {
    if (!(args && args.length)) {
      return value;
    }
    const format = [...args];
    const pipeToken = format.splice(0, 1)[0];
    if (DynamicPipe.PIPE_MAPPINGS[pipeToken]) {
      return DynamicPipe.PIPE_MAPPINGS[pipeToken].transform(value, ...format);
    }
  }
}
