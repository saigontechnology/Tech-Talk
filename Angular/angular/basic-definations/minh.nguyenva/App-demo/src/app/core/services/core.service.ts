import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, filter, map } from 'rxjs';
import { HttpService } from './http.service';
import { LocationSiteResponse, FuelTypesReponse, LocationSite, FuelTypes, Country, CountryData } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class CoreService {
  constructor(private _httpService: HttpService) {}
  private _locationSite = new BehaviorSubject<any>({});
  private _fuelTypes = new BehaviorSubject<any>({});
  private _selectedLocation = new BehaviorSubject<any>({});
  private _countries = new BehaviorSubject<any>({});

  locationSite$ = this._locationSite.asObservable().pipe(
    filter((location) => !!location.loaded),
    map((location) => location.content)
  );

  fuelTypes$ = this._fuelTypes.asObservable().pipe(
    filter((fuelTypes) => !!fuelTypes.loaded),
    map((fuelTypes) => fuelTypes.content)
  );

  countries$ = this._countries.asObservable().pipe(
    filter((contries) => !!contries.loaded),
    map((contries) => contries.content)
  );

  selectedLocation$ = this._selectedLocation.asObservable();

  getLocationSite(): Observable<LocationSiteResponse> {
    return this._httpService.getResource('location-site/user');
  }

  getFuelTypes(): Observable<FuelTypesReponse> {
    return this._httpService.getResource('fuel-types');
  }

  getCountries(): Observable<Country> {
    return this._httpService.getResource('countries');
  }

  setLocationSite(locationSite: LocationSite) {
    this._locationSite.next(locationSite);
  }

  setFuelTypes(fuelTypes: FuelTypes) {
    this._fuelTypes.next(fuelTypes);
  }

  setCountries(countries: CountryData) {
    this._countries.next(countries);
  }

  setSelectedLocation(selectedLocation?: LocationSiteResponse) {
    this._selectedLocation.next(selectedLocation);
  }
}
