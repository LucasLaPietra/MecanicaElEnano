import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { CuotasModule } from 'src/cuotas/cuotas.module';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { OrdenTrabajoComponent } from './orden-trabajo.component';
import { OrdenTrabajoPrintComponent } from './orden-trabajo-print/orden-trabajo-print.component';
import { MatCardModule } from '@angular/material/card';



@NgModule({
  declarations: [
    OrdenTrabajoComponent,
    OrdenTrabajoPrintComponent
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
    MatCardModule,
    MatInputModule,
    MatSortModule,
    MatToolbarModule,
    MatDatepickerModule,
    MatIconModule,
    MatOptionModule,
    MatSelectModule,
    CuotasModule,
    RouterModule,
    MatSidenavModule
  ],
  exports:[
    OrdenTrabajoComponent
  ]
})
export class OrdenTrabajoModule { }
