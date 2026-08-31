import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

import { CustomersService } from '../../customers.service';
import { Customer } from '../../models';
import { CreateCustomerComponent } from '../create-customer/create-customer.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  readonly displayedColumns = ['firstName', 'lastName', 'email', 'phoneNumber', 'createdDate'];
  readonly pageSizeOptions = [10, 20, 50];

  dataSource: Customer[] = [];

  totalCount = 0;
  pageNumber = 1;
  pageSize = 20;
  isLoading = false;

  constructor(
    private customersService: CustomersService,
    private dialog: MatDialog
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
}
