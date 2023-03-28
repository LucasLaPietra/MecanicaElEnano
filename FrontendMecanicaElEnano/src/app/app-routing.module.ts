import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PresupuestoComponent } from 'src/presupuesto/presupuesto.component';
import { VehiculoComponent } from 'src/vehiculo/vehiculo.component';

const routes: Routes = [
  { path: 'presupuestos/:id', component: PresupuestoComponent },
  { path: '', component: VehiculoComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
