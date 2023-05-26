import { FormGroup } from "@angular/forms";

export namespace FormHelper {

    export function validateFormGroup(formGroup: FormGroup): boolean {
        let isValid = true;

        for (const key in formGroup.controls) {
            if (formGroup.controls.hasOwnProperty(key)) {
                const formControl = formGroup.controls[key];
                formControl.markAsDirty();
                formControl.updateValueAndValidity();

                if (formControl.errors) {
                    isValid = false;
                }
            }
        }

        return isValid;
    }
}