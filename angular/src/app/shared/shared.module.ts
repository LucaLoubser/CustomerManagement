import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationModalComponent } from './components/confirmation-modal/confirmation-modal.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';


@NgModule({
  declarations: [
    ConfirmationModalComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule
  ]
})
export class SharedModule { }
