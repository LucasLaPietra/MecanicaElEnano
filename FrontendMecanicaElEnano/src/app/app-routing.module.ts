import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdenTrabajoPrintComponent } from 'src/orden-trabajo/orden-trabajo-print/orden-trabajo-print.component';
import { PresupuestoPrintComponent } from 'src/presupuesto/presupuesto-print/presupuesto-print.component';
import { TrabajoPrintComponent } from 'src/trabajo/trabajo-print/trabajo-print.component';

const routes: Routes = [
  {
    path: 'presupuestos/:id',
    loadChildren: () => import('src/presupuesto/presupuesto.module').then(m => m.PresupuestoModule)
  },
  {
    path: 'presupuestos/:id/:idTrabajo',
    loadChildren: () => import('src/presupuesto/presupuesto.module').then(m => m.PresupuestoModule)
  },
  {
    path: 'presupuestos/ordenTrabajo/:id/:idOrdenTrabajo',
    loadChildren: () => import('src/presupuesto/presupuesto.module').then(m => m.PresupuestoModule)
  },
  { path: 'presupuestos-imprimir/:idVehiculo/:idPresupuesto', component: PresupuestoPrintComponent },
  {
    path: 'trabajos/:id',
    loadChildren: () => import('src/trabajo/trabajo.module').then(m => m.TrabajoModule)
  },
  {
    path: 'trabajos/:id/:idPresupuesto/:idTrabajo',
    loadChildren: () => import('src/trabajo/trabajo.module').then(m => m.TrabajoModule)
  },
  { path: 'trabajos-imprimir/:idVehiculo/:idTrabajo', component: TrabajoPrintComponent },
  {
    path: 'ordentrabajos/:id',
    loadChildren: () => import('src/orden-trabajo/orden-trabajo.module').then(m => m.OrdenTrabajoModule)
  },
  {
    path: 'ordentrabajos/:id/:idPresupuesto/:idOrdenTrabajo',
    loadChildren: () => import('src/orden-trabajo/orden-trabajo.module').then(m => m.OrdenTrabajoModule)
  },
  {
    path: 'ordentrabajos/:id/:idPresupuesto',
    loadChildren: () => import('src/orden-trabajo/orden-trabajo.module').then(m => m.OrdenTrabajoModule)
  },
  { path: 'ordentrabajos-imprimir/:idVehiculo/:idOrdenTrabajo', component: OrdenTrabajoPrintComponent },
  {
    path: 'turnos',
    loadChildren: () => import('src/turnos/turnos.module').then(m => m.TurnosModule)
  },
  {
    path: 'vehiculos',
    loadChildren: () => import('src/vehiculo/vehiculo.module').then(m => m.VehiculoModule)
  },
  { path: '', redirectTo: 'turnos', pathMatch: 'full' },
  { path: '**', redirectTo: "turnos" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
