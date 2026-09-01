import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

import { CustomersService } from '../../customers.service';
import { Customer } from '../../models';
import { CreateCustomerComponent } from '../create-customer/create-customer.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { toErrorMessage } from 'src/app/shared/problem-details.utils';
import { HttpErrorResponse } from '@angular/common/http';
import { ConfirmationModalComponent } from 'src/app/shared/components/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  readonly displayedColumns = ['firstName', 'lastName', 'email', 'phoneNumber', 'createdDate', 'actions'];
  readonly pageSizeOptions = [10, 20, 50];

  dataSource: Customer[] = [];

  totalCount = 0;
  pageNumber = 1;
  pageSize = 20;
  isLoading = false;

  constructor(
    private customersService: CustomersService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadPage();
  }

  openCreateDialog(): void {
    this.dialog
      .open(CreateCustomerComponent, { width: '360px', disableClose: true })
      .afterClosed()
      .subscribe(created => {
        if (created) { this.loadPage(); }
      });
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadPage();
  }

  private loadPage(): void {
    this.isLoading = true;

    this.customersService
      .getPagedCustomerList(this.pageNumber, this.pageSize)
      .subscribe({
        next: result => {
          this.dataSource = result.items;
          this.totalCount = result.totalCount;
          this.pageNumber = result.pageNumber;
          this.pageSize = result.pageSize;
          this.isLoading = false;
        },
        error: () => {
          this.dataSource = [];
          this.totalCount = 0;
          this.isLoading = false;
        }
      });
  }

  private deleteCustomer(id: string): void {
    this.customersService.deleteCustomer(id)
      .subscribe({
        next: () => {
          this.snackBar.open("Customer Successfully deleted", 'Dismiss', {
            duration: 6000,
            panelClass: 'snack-success'
          });
          this.loadPage();
        },
        error: (err: HttpErrorResponse) => {
          this.snackBar.open(toErrorMessage(err), 'Dismiss', {
            duration: 6000,
            panelClass: 'snack-error'
          });
      }});
  }

  confirmDelete(customer: Customer) : void {
    var confirmationModalData = {
      title: "Delete Customer",
      message: `Do you want to delete ${customer.firstName} ${customer.lastName} - ${customer.email} ?`,
      confirmText: "Delete",
      cancelText: "Cancel"
    }

    this.dialog
    .open(ConfirmationModalComponent, { width: '360px', disableClose: true, data: confirmationModalData})
    .afterClosed()
    .subscribe(confirmed => {
      if(confirmed == true)
        this.deleteCustomer(customer.id);
    });

  }
}
