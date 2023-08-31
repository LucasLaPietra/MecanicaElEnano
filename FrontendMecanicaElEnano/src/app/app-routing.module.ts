import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PresupuestoComponent } from 'src/presupuesto/presupuesto.component';
import { TurnosComponent } from 'src/turnos/turnos.component';
import { VehiculoComponent } from 'src/vehiculo/vehiculo.component';

const routes: Routes = [
  { path: 'presupuestos/:id', component: PresupuestoComponent },
  { path: 'turnos', component: TurnosComponent },
  { path: 'vehiculos', component: VehiculoComponent },
  { path: 'turnos/:date/:id', component: TurnosComponent },
  { path: 'vehiculos/:date', component: VehiculoComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
