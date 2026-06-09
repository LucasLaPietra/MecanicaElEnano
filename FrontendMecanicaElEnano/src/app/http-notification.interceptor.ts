import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable()
export class HttpNotificationInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      tap((event) => {
        if (!(event instanceof HttpResponse)) {
          return;
        }

        const message = this.buildSuccessMessage(req, event);

        if (!message) {
          return;
        }

        this.showToast(message, 'success');
      }),
      catchError((error: HttpErrorResponse) => {
        const message = this.buildErrorMessage(error);
        this.showToast(message, 'error');

        return throwError(() => error);
      })
    );
  }

  private showToast(message: string, type: 'success' | 'error'): void {
    this.snackBar.open(message, type === 'success' ? 'OK' : 'Cerrar', {
      duration: type === 'success' ? 3000 : 5000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: type === 'success' ? ['toast-success'] : ['toast-error'],
    });
  }

  private buildErrorMessage(error: HttpErrorResponse): string {
    if (error.error && typeof error.error === 'string') {
      return error.error;
    }

    if (error.error && typeof error.error === 'object' && 'message' in error.error) {
      return String(error.error.message);
    }

    if (error.status === 0) {
      return 'No se pudo conectar con el servidor.';
    }

    return `Error ${error.status}: ${error.statusText || 'Error inesperado'}`;
  }

  private buildSuccessMessage(req: HttpRequest<unknown>, event: HttpResponse<unknown>): string | null {
    if (!event.ok || (req.method !== 'POST' && req.method !== 'PUT' && req.method !== 'DELETE')) {
      return null;
    }

    const entity = this.resolveEntityName(req.url);

    if (!entity) {
      return null;
    }

    if (req.method === 'POST') {
      return `${entity} creado correctamente.`;
    }

    if (req.method === 'PUT') {
      return `${entity} modificado correctamente.`;
    }

    return `${entity} eliminado correctamente.`;
  }

  private resolveEntityName(url: string): string | null {
    const pathSegments = url.split('?')[0].split('/').filter(Boolean);
    const apiIndex = pathSegments.findIndex((segment) => segment.toLowerCase() === 'api');

    if (apiIndex === -1 || apiIndex + 1 >= pathSegments.length) {
      return null;
    }

    const resource = pathSegments[apiIndex + 1].toLowerCase();

    switch (resource) {
      case 'presupuestos':
        return 'Presupuesto';
      case 'trabajos':
        return 'Trabajo';
      case 'ordentrabajos':
        return 'Orden de trabajo';
      case 'turnos':
        return 'Turno';
      case 'vehiculos':
        return 'Vehiculo';
      default:
        return null;
    }
  }
}