import { Component, Inject } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { CustomersService } from '../../customers.service';
import { Customer } from '../../models';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { toErrorMessage } from '../../../shared/problem-details.utils';
import { ConfirmationModalComponent } from 'src/app/shared/components/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-customer-modal',
  templateUrl: './customer-modal.component.html',
  styleUrls: ['./customer-modal.component.css'],
})
export class CustomerModalComponent {
  createCustomerForm = new FormGroup({
    firstName: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    lastName: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    email: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    phoneNumber:new FormControl(''),
  });

  constructor(
    private customersService: CustomersService,
    private dialogRef: MatDialogRef<CustomerModalComponent, Customer>,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null
  ){
    if(this.data != null){
      this.createCustomerForm.patchValue(this.data);
    }
  }

  isSaving: boolean = false;
  isEditing() {
    return this.data != null;
  }

  onSubmit(): void {
    if (this.createCustomerForm.invalid) { return; }
    this.isSaving = true;

    const customer = this.createCustomerForm.getRawValue();
    var requestBody = {...customer, phoneNumber: customer.phoneNumber || null};

    const request$ = this.isEditing()
      ? this.customersService.updateCustomer(this.data!.id, requestBody)
      : this.customersService.createCustomer(requestBody);

    request$
      .subscribe({
        next: created => {
          this.dialogRef.close(created)
          this.snackBar.open(`Customer Successfully ${this.isEditing() ? "Updated" : "Created"}`, 'Dismiss', {
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

  isDirty() : boolean {
    var newCustomer = this.createCustomerForm.controls;

    if(!this.isEditing()){
      return !(this.isNullOrWhiteSpace(newCustomer.firstName.value)
        && this.isNullOrWhiteSpace(newCustomer.lastName.value)
        && this.isNullOrWhiteSpace(newCustomer.email.value)
        && this.isNullOrWhiteSpace(newCustomer.phoneNumber.value)
      )
    }

    var oldCustomer = this.data;
    if(oldCustomer != null){
      return oldCustomer.firstName != newCustomer.firstName.value
        || oldCustomer.lastName != newCustomer.lastName.value
        || oldCustomer.email != newCustomer.email.value
        || oldCustomer.phoneNumber != newCustomer.phoneNumber.value;
    }

    return false;
  }

  onCancel() : void {
    console.log("cancel pressed")
    if(this.isDirty()){
      var confirmationModalData = {
        title: "Unsaved Changes",
        message: "You may have unsaved changes are you sure you want to cancel ?",
        confirmText: "Continue to Cancel",
        cancelText: "Go Back"
      }

      this.dialog
        .open(ConfirmationModalComponent, { width: '360px', disableClose: true, data: confirmationModalData})
        .afterClosed()
        .subscribe(confirmed => {
          if(confirmed == true)
            this.dialogRef.close();
        });
    }else{
      this.dialogRef.close();
    }
  }

  isNullOrWhiteSpace(str: string | null | undefined): boolean {
    return !str
      || str.trim() === "";
  }
}
