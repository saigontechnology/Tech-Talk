export interface Person {
  name_person: string;
  age_of_person: number;
  address_street: string;
  address_city: string;
  address_zip_code: number;
}

export interface Person2 {
  name: string;
  age: number;
  street: string;
  city: string;
  zipCode: number;
}

// Utility Type
export interface Person3 extends Pick<Person2, 'name' | 'age'> {
  country: string;
  lastName: string;
  firstName: string;
}

export interface Person4 {
  [key: string]: any;
}

export interface Person5 extends Person4 {
  id: string;
}

export interface TimeZone {
  DaylightDelta: number;
  DaylightEndHours: number;
  DaylightEndMinutes: number;
  DaylightEndMonth: number;
  DaylightEndWeekDay: number;
  DaylightEndWeekInMonth: number;
  DaylightName: string;
  DaylightStartHours: number;
  DaylightStartMinutes: number;
  DaylightStartMonth: number;
  DaylightStartWeekDay: number;
  DaylightStartWeekInMonth: number;
  Description: unknown;
}
