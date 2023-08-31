import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TurnosComponent } from './turnos.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    TurnosComponent
  ],
  imports: [
    CommonModule,
    MatPaginatorModule,
    MatButtonToggleModule,
    MatTableModule,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatSortModule,
    MatToolbarModule,
    MatDatepickerModule,
    MatIconModule,
    MatOptionModule,
    MatSelectModule,
    MatCardModule,
    RouterModule,
  ],
  exports:[
    TurnosComponent
  ]
})
export class TurnosModule { }
