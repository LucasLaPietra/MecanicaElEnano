import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrdenTrabajo } from 'src/domain/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdenTrabajosService {
  private readonly apiUrl = `${environment.apiBaseUrl}/ordenTrabajos`;
  private readonly guidJsonOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient:HttpClient) { }

  public CreateOrdenTrabajo(vehiculoId:string): Observable<OrdenTrabajo> {
    return this.httpClient.post<OrdenTrabajo>(this.apiUrl, JSON.stringify(vehiculoId), this.guidJsonOptions)
  }

  public GetOrdenTrabajos(): Observable<OrdenTrabajo[]> {
    return this.httpClient.get<OrdenTrabajo[]>(this.apiUrl);
  }

  public GetOrdenTrabajo(id:string): Observable<OrdenTrabajo> {
    return this.httpClient.get<OrdenTrabajo>(`${this.apiUrl}/${id}`);
  }

  public UpdateOrdenTrabajo(ordenOrdenTrabajo:OrdenTrabajo): Observable<OrdenTrabajo> {
    return this.httpClient.put<OrdenTrabajo>(this.apiUrl, ordenOrdenTrabajo)
  }

  public DeleteOrdenTrabajo(id:string): Observable<OrdenTrabajo> {
    return this.httpClient.delete<OrdenTrabajo>(`${this.apiUrl}/${id}`);
  }
}
