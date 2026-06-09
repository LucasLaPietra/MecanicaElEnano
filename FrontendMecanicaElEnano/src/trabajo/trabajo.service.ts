import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Trabajo } from 'src/domain/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TrabajosService {
  private readonly apiUrl = `${environment.apiBaseUrl}/trabajos`;
  private readonly guidJsonOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient:HttpClient) { }

  public CreateTrabajo(vehiculoId:string): Observable<Trabajo> {
    return this.httpClient.post<Trabajo>(this.apiUrl, JSON.stringify(vehiculoId), this.guidJsonOptions)
  }

  public GetTrabajos(): Observable<Trabajo[]> {
    return this.httpClient.get<Trabajo[]>(this.apiUrl);
  }

  public GetTrabajo(id:string): Observable<Trabajo> {
    return this.httpClient.get<Trabajo>(`${this.apiUrl}/${id}`);
  }

  public UpdateTrabajo(trabajo:Trabajo): Observable<Trabajo> {
    return this.httpClient.put<Trabajo>(this.apiUrl, trabajo)
  }

  public DeleteTrabajo(id:string): Observable<Trabajo> {
    return this.httpClient.delete<Trabajo>(`${this.apiUrl}/${id}`);
  }
}
