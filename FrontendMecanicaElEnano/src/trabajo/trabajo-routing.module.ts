import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TrabajoComponent } from './trabajo.component';

const routes: Routes = [
  { path: '', component: TrabajoComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TrabajoRoutingModule {}
