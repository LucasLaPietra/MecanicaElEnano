import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Presupuesto } from 'src/domain/entities';

@Injectable({
  providedIn: 'root'
})
export class PresupuestosService {

  constructor(private httpClient:HttpClient) { }

  public CreatePresupuesto(vehiculoId:string): Observable<Presupuesto> {
    return this.httpClient.post<Presupuesto>('https://localhost:7009/api/presupuestos' + "?vehiculoId=" + vehiculoId, "")
  }

  public GetPresupuestos(): Observable<Presupuesto[]> {
    return this.httpClient.get<Presupuesto[]>('https://localhost:7009/api/presupuestos');
  }

  public GetPresupuesto(id:string): Observable<Presupuesto> {
    return this.httpClient.get<Presupuesto>('https://localhost:7009/api/presupuestos/' + id);
  } 

  public UpdatePresupuesto(presupuesto:Presupuesto): Observable<Presupuesto> {
    return this.httpClient.put<Presupuesto>('https://localhost:7009/api/presupuestos', presupuesto)
  }

  public DeletePresupuesto(id:string): Observable<Presupuesto> {
    return this.httpClient.delete<Presupuesto>('https://localhost:7009/api/presupuestos/' + id);
  }
}