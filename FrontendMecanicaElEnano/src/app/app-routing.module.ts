import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PresupuestoComponent } from 'src/presupuesto/presupuesto.component';
import { TrabajoComponent } from 'src/trabajo/trabajo.component';
import { TurnosComponent } from 'src/turnos/turnos.component';
import { VehiculoComponent } from 'src/vehiculo/vehiculo.component';

const routes: Routes = [
  { path: 'presupuestos/:id', component: PresupuestoComponent },
  { path: 'presupuestos/:id/:idTrabajo', component: PresupuestoComponent },
  { path: 'presupuestos/:id/ordenTrabajo/:idOrdenTrabajo', component: PresupuestoComponent },
  { path: 'trabajos/:id', component: TrabajoComponent },
  { path: 'trabajos/:id/:idPresupuesto/:idTrabajo', component: TrabajoComponent },
  { path: 'ordentrabajos/:id', component: TrabajoComponent },
  { path: 'ordentrabajos/:id/:idPresupuesto/:idOrdenTrabajo', component: TrabajoComponent },
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
