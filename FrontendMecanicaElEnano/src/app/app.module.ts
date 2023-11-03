import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VehiculoModule } from 'src/vehiculo/vehiculo.module';
import { HttpClientModule } from '@angular/common/http';
import { PresupuestoModule } from 'src/presupuesto/presupuesto.module';
import { MAT_DATE_LOCALE, MatNativeDateModule } from '@angular/material/core';
import { TurnosModule } from 'src/turnos/turnos.module';
import { TrabajoModule } from 'src/trabajo/trabajo.module';
import { OrdenTrabajoModule } from 'src/orden-trabajo/orden-trabajo.module';
import { CancelModalModule } from 'src/cancel-modal/cancel-modal.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatNativeDateModule,
    VehiculoModule,
    PresupuestoModule,
    TurnosModule,
    HttpClientModule,
    TrabajoModule,
    OrdenTrabajoModule,
    CancelModalModule
  ],
  providers: [
    {provide: MAT_DATE_LOCALE, useValue: 'es-ES'}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
