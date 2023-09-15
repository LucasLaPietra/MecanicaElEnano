import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrdenTrabajo } from 'src/domain/entities';

@Injectable({
  providedIn: 'root'
})
export class OrdenTrabajosService {

  constructor(private httpClient:HttpClient) { }

  public CreateOrdenTrabajo(vehiculoId:string): Observable<OrdenTrabajo> {
    return this.httpClient.post<OrdenTrabajo>('https://localhost:7009/api/ordenTrabajos' + "?vehiculoId=" + vehiculoId, "")
  }

  public GetOrdenTrabajos(): Observable<OrdenTrabajo[]> {
    return this.httpClient.get<OrdenTrabajo[]>('https://localhost:7009/api/ordenTrabajos');
  }

  public GetOrdenTrabajo(id:string): Observable<OrdenTrabajo> {
    return this.httpClient.get<OrdenTrabajo>('https://localhost:7009/api/ordenTrabajos/' + id);
  }

  public UpdateOrdenTrabajo(ordenOrdenTrabajo:OrdenTrabajo): Observable<OrdenTrabajo> {
    return this.httpClient.put<OrdenTrabajo>('https://localhost:7009/api/ordenTrabajos', ordenOrdenTrabajo)
  }

  public DeleteOrdenTrabajo(id:string): Observable<OrdenTrabajo> {
    return this.httpClient.delete<OrdenTrabajo>('https://localhost:7009/api/ordenTrabajos/' + id);
  }
}
