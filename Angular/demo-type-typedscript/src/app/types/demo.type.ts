import {
  Person2,
  Person3,
  Person4,
  Person5,
} from '../interfaces/demo.interface';

export type CamelCase<S extends string> =
  S extends `${infer P1}_${infer P2}${infer P3}`
    ? `${Lowercase<P1>}${Uppercase<P2>}${CamelCase<P3>}`
    : Lowercase<S>;

/* const camelCase: CamelCase<'hello_world_example'>
 result: 
    this.camelCase = 'helloworldexample'; => error 
    this.camelCase = 'helloWorldExample';

 */

export type KeysToCamelCase<T> = {
  [K in keyof T as CamelCase<string & K>]: T[K];
};

export type KeysToUpperCase<T> = {
  [K in keyof T as Capitalize<string & K>]: T[K];
};

//Conditional Types
export type PersonHasCountry = Person3 extends Person2
  ? { country1: string }
  : boolean;
// result: type PersonHasCountry = boolean;

export type PersonHasId = Person5 extends Person4
  ? { country1: string }
  : boolean;
// result: type PersonHasCountry = { country1: string };
