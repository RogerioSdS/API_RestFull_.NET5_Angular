import { AbstractControl, FormGroup } from "@angular/forms";

export class ValidatorField {
  static MustMatch(controlName: string, matchingControlName: string): any {
    return (group: AbstractControl) => {
      const formGroup = group as FormGroup;
      const control = formGroup.controls[controlName];
      const matchingcontrol = formGroup.controls[matchingControlName];

      if (matchingcontrol.errors && !matchingcontrol.errors['mustMatch']) {
        return null;
      }

      if (control.value !== matchingcontrol.value) {
        matchingcontrol.setErrors({ mustMatch: true });
      } else {
        matchingcontrol.setErrors(null);
      }
      return null;
    };
  }
}
