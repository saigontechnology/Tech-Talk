import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { DATE_FORMAT } from '../app-enum';
import { Coordinate } from 'ol/coordinate';
import { equals } from 'ol/coordinate';

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor() { }
  
  public checkValidCoordinate(coordinate: Coordinate){
    const [lng, lat] = coordinate;
    if (isNaN(lng) || isNaN(lat)){
      return false;
    }
    if (
      !equals(coordinate, [0, 0]) &&
      coordinate[0] >= -180 &&
      coordinate[0] <= 180 &&
      coordinate[1] >= -90 &&
      coordinate[1] <= 90
    ){
      return true;
    }
    return false
  }

  public formatDateTime (data:Date, format :any){
    switch(format) { 
      case DATE_FORMAT.DATE_MONTH: { 
       return formatDate(data, 'dd/MM', 'en-US');
      } 
      case DATE_FORMAT.YEAR: { 
        return formatDate(data, 'YYYY', 'en-US'); 
      } 
      case DATE_FORMAT.SHORT_MONTH_NAME: { 
        return data.toLocaleString('default', { month: 'short' });
      } 
      case DATE_FORMAT.QUARTERLY: {
        return this._getQuaterly(data).toString();
      }
      case DATE_FORMAT.DATE_TIME_SUFFIX:{
        return data.getDate() + this._daySuffix(+ data.getDate()) + ' ' + data.toLocaleString('default', { month: 'long' }) + ' ' +  data.getFullYear();
      }
      case DATE_FORMAT.SHORT_TIME:{
        return data.toLocaleTimeString('en-US', { hour12: true, hour: '2-digit', minute: '2-digit' });
      }
      default: { 
        return formatDate(data, 'dd/MM/YYYY', 'en-US'); 
      } 
   } 
  }

  private _getQuaterly(date:Date){
    return Math.floor(date.getMonth() / 3 + 1);
  }

  private _daySuffix(number: number) {
    var d = number % 10;
    return ~~((number % 100) / 10) === 1 ? 'th' : d === 1 ? 'st' : d === 2 ? 'nd' : d === 3 ? 'rd' : 'th';
  }
}