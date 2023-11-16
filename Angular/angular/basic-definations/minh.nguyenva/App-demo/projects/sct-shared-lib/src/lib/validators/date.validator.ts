import { AbstractControl, ValidationErrors } from '@angular/forms';
export class DateValidator {
  static FULL_DATE(control: AbstractControl): ValidationErrors | null {
    if (typeof control.value === 'string') {
      return { ngbdate: true };
    }
    if ((control.value.year && control.value.year < 2000) || control.value.year > 2048) {
      return { year_invalid: true };
    }
    return null;
  }
}
