import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdenTrabajoPrintComponent } from 'src/orden-trabajo/orden-trabajo-print/orden-trabajo-print.component';
import { OrdenTrabajoComponent } from 'src/orden-trabajo/orden-trabajo.component';
import { PresupuestoPrintComponent } from 'src/presupuesto/presupuesto-print/presupuesto-print.component';
import { PresupuestoComponent } from 'src/presupuesto/presupuesto.component';
import { TrabajoPrintComponent } from 'src/trabajo/trabajo-print/trabajo-print.component';
import { TrabajoComponent } from 'src/trabajo/trabajo.component';
import { TurnosComponent } from 'src/turnos/turnos.component';
import { VehiculoComponent } from 'src/vehiculo/vehiculo.component';

const routes: Routes = [
  { path: 'presupuestos/:id', component: PresupuestoComponent },
  { path: 'presupuestos/:id/:idTrabajo', component: PresupuestoComponent },
  { path: 'presupuestos/ordenTrabajo/:id/:idOrdenTrabajo', component: PresupuestoComponent },
  { path: 'presupuestos-imprimir/:idVehiculo/:idPresupuesto', component: PresupuestoPrintComponent },
  { path: 'trabajos/:id', component: TrabajoComponent },
  { path: 'trabajos/:id/:idPresupuesto/:idTrabajo', component: TrabajoComponent },
  { path: 'trabajos-imprimir/:idVehiculo/:idTrabajo', component: TrabajoPrintComponent },
  { path: 'ordentrabajos/:id', component: OrdenTrabajoComponent },
  { path: 'ordentrabajos/:id/:idPresupuesto/:idOrdenTrabajo', component: OrdenTrabajoComponent },
  { path: 'ordentrabajos/:id/:idPresupuesto', component: OrdenTrabajoComponent },
  { path: 'ordentrabajos-imprimir/:idVehiculo/:idOrdenTrabajo', component: OrdenTrabajoPrintComponent },
  { path: 'turnos', component: TurnosComponent },
  { path: 'vehiculos', component: VehiculoComponent },
  { path: 'turnos/:date/:id', component: TurnosComponent },
  { path: 'vehiculos/:date', component: VehiculoComponent },
  { path: '**', redirectTo: "turnos" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
