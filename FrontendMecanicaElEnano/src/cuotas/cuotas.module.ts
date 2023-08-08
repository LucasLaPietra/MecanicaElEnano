import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CuotasComponent } from './cuotas.component';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';



@NgModule({
  declarations: [
    CuotasComponent
  ],
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule
  ],
  exports: [
    CuotasComponent
  ]
})
export class CuotasModule { }
