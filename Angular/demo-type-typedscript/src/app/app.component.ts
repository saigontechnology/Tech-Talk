import { Component } from '@angular/core';
import {
  CamelCase,
  KeysToCamelCase,
  KeysToUpperCase,
  PersonHasCountry,
  PersonHasId,
} from './types/demo.type';
import {
  Person,
  Person2,
  Person3,
  Person4,
  TimeZone,
} from './interfaces/demo.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
})
export class AppComponent {
  camelCase!: CamelCase<'hello_world_example'>;
  personInformationCamelCase!: KeysToCamelCase<Person>;
  personInformationUpperCase!: KeysToUpperCase<Person2>;
  person3!: Person3;
  person4!: Person4;
  personHasId!: PersonHasId;

  ngOnInit(): void {
    this.personInformationCamelCase = {
      namePerson: 'test 1',
      ageOfPerson: 22,
      addressStreet: 'street-1',
      addressCity: 'hcm',
      addressZipCode: 456,
    };

    this.personInformationUpperCase = {
      Name: 'test 1',
      Age: 22,
      Street: 'street-1',
      City: 'hcm',
      ZipCode: 456,
    };

    this.person3 = {
      name: 'test 3',
      age: 33,
      country: 'VN',
      firstName: 'test',
      lastName: 'test',
    };

    this.person4 = {
      name: 'test 4',
      date_of_birthday: '01/01/1999',
      Phone: 123455,
    };

    this.personHasId = {
      country1: 'country',
    };

    // this.getActiveTimeZones('Name'); => error: '"Name"' is not assignable to parameter of type 'keyof TimeZone'
    this.getActiveTimeZones('DaylightDelta');
  }

  getActiveTimeZones(orderBy: keyof TimeZone): string {
    return '';
  }
}
