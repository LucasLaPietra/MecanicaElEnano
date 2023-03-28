import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CuotasComponent } from './cuotas.component';
import { MatTableModule } from '@angular/material/table';



@NgModule({
  declarations: [
    CuotasComponent
  ],
  imports: [
    CommonModule,
    MatTableModule
  ],
  exports: [
    CuotasComponent
  ]
})
export class CuotasModule { }
