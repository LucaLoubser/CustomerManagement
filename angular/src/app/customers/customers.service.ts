import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Customer, PagedResult } from './models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/CustomerManagement`;

  constructor(
    private http: HttpClient
  ){}

  getPagedCustomerList(
    pageNumber = 1,
    pageSize = 20,
    filter?: string
  ): Observable<PagedResult<Customer>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (filter != null) {
      params = params.set('filter', filter);
    }

    return this.http
      .get<PagedResult<Customer>>(this.baseUrl, { params });
  }
}
