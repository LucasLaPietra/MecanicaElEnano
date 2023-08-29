import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Trabajo } from 'src/domain/entities';

@Injectable({
  providedIn: 'root'
})
export class TrabajosService {

  constructor(private httpClient:HttpClient) { }

  public CreateTrabajo(trabajo:Trabajo): Observable<Trabajo> {
    return this.httpClient.post<Trabajo>('https://localhost:7009/api/trabajos',trabajo)
  }

  public GetTrabajos(): Observable<Trabajo[]> {
    return this.httpClient.get<Trabajo[]>('https://localhost:7009/api/trabajos');
  }

  public GetTrabajo(id:string): Observable<Trabajo> {
    return this.httpClient.get<Trabajo>('https://localhost:7009/api/trabajos/' + id);
  } 

  public UpdateTrabajo(trabajo:Trabajo): Observable<Trabajo> {
    return this.httpClient.put<Trabajo>('https://localhost:7009/api/trabajos', trabajo)
  }

  public DeleteTrabajo(id:string): Observable<Trabajo> {
    return this.httpClient.delete<Trabajo>('https://localhost:7009/api/trabajos/' + id);
  }
}