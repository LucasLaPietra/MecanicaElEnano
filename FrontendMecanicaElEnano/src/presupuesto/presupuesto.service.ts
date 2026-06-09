import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Presupuesto } from 'src/domain/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PresupuestosService {
  private readonly apiUrl = `${environment.apiBaseUrl}/presupuestos`;
  private readonly guidJsonOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient:HttpClient) { }

  public CreatePresupuesto(vehiculoId:string): Observable<Presupuesto> {
    return this.httpClient.post<Presupuesto>(this.apiUrl, JSON.stringify(vehiculoId), this.guidJsonOptions)
  }

  public GetPresupuestos(): Observable<Presupuesto[]> {
    return this.httpClient.get<Presupuesto[]>(this.apiUrl);
  }

  public GetPresupuesto(id:string): Observable<Presupuesto> {
    return this.httpClient.get<Presupuesto>(`${this.apiUrl}/${id}`);
  } 

  public UpdatePresupuesto(presupuesto:Presupuesto): Observable<Presupuesto> {
    return this.httpClient.put<Presupuesto>(this.apiUrl, presupuesto)
  }

  public DeletePresupuesto(id:string): Observable<Presupuesto> {
    return this.httpClient.delete<Presupuesto>(`${this.apiUrl}/${id}`);
  }
}