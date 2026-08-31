import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { CustomersService } from '../../customers.service';
import { Customer } from '../../models';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { toErrorMessage } from '../../../shared/problem-details.utils';

@Component({
  selector: 'app-create-customer',
  templateUrl: './create-customer.component.html',
  styleUrls: ['./create-customer.component.css'],
})
export class CreateCustomerComponent {
  createCustomerForm = new FormGroup({
    firstName: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    lastName: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    email: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    phoneNumber:new FormControl(''),
  });

  constructor(
    private customersService: CustomersService,
    private dialogRef: MatDialogRef<CreateCustomerComponent, Customer>,
    private snackBar: MatSnackBar
  ) { }

  isSaving: boolean = false;
  serverError: string | null = null;

  onSubmit(): void {
    if (this.createCustomerForm.invalid) { return; }
    this.isSaving = true;

    const customer = this.createCustomerForm.getRawValue();

    this.customersService.createCustomer({...customer, phoneNumber: customer.phoneNumber || null})
      .subscribe({
        next: created => {
          this.dialogRef.close(created)
          this.snackBar.open("Customer Successfully created", 'Dismiss', {
            duration: 6000,
            panelClass: 'snack-success'
          });
        },
        error: (err: HttpErrorResponse) => {
          this.isSaving = false;
          this.snackBar.open(toErrorMessage(err), 'Dismiss', {
            duration: 6000,
            panelClass: 'snack-error'
          });
      }});
  }
}
