import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdenTrabajoComponent } from './orden-trabajo.component';

const routes: Routes = [
  { path: '', component: OrdenTrabajoComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrdenTrabajoRoutingModule {}
