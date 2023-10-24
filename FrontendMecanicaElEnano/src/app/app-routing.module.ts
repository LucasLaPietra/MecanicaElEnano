import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdenTrabajoComponent } from 'src/orden-trabajo/orden-trabajo.component';
import { PresupuestoComponent } from 'src/presupuesto/presupuesto.component';
import { TrabajoComponent } from 'src/trabajo/trabajo.component';
import { TurnosComponent } from 'src/turnos/turnos.component';
import { VehiculoComponent } from 'src/vehiculo/vehiculo.component';

const routes: Routes = [
  { path: 'presupuestos/:id', component: PresupuestoComponent },
  { path: 'presupuestos/:id/:idTrabajo', component: PresupuestoComponent },
  { path: 'presupuestos/ordenTrabajo/:id/:idOrdenTrabajo', component: PresupuestoComponent },
  { path: 'trabajos/:id', component: TrabajoComponent },
  { path: 'trabajos/:id/:idPresupuesto/:idTrabajo', component: TrabajoComponent },
  { path: 'ordentrabajos/:id', component: OrdenTrabajoComponent },
  { path: 'ordentrabajos/:id/:idPresupuesto/:idOrdenTrabajo', component: OrdenTrabajoComponent },
  { path: 'ordentrabajos/:id/:idPresupuesto', component: OrdenTrabajoComponent },
  { path: 'turnos', component: TurnosComponent },
  { path: 'vehiculos', component: VehiculoComponent },
  { path: 'turnos/:date/:id', component: TurnosComponent },
  { path: 'vehiculos/:date', component: VehiculoComponent },
  { path: '**', component: TurnosComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
