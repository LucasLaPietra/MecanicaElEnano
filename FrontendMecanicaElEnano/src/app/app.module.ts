import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { MAT_DATE_LOCALE, MatNativeDateModule } from '@angular/material/core';
import { CancelModalModule } from 'src/cancel-modal/cancel-modal.module';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { HttpNotificationInterceptor } from './http-notification.interceptor';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatNativeDateModule,
    HttpClientModule,
    MatSnackBarModule,
    CancelModalModule
  ],
  providers: [
    {provide: MAT_DATE_LOCALE, useValue: 'es-ES'},
    {provide: HTTP_INTERCEPTORS, useClass: HttpNotificationInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
