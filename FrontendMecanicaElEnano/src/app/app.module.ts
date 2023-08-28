import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VehiculoModule } from 'src/vehiculo/vehiculo.module';
import { HttpClientModule } from '@angular/common/http';
import { PresupuestoModule } from 'src/presupuesto/presupuesto.module';
import { MatNativeDateModule } from '@angular/material/core';
import { TurnosModule } from 'src/turnos/turnos.module';

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
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
