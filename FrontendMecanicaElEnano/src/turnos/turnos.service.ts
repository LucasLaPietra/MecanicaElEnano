import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Turno } from 'src/domain/entities';

@Injectable({
  providedIn: 'root'
})
export class TurnosService {

  constructor(private httpClient:HttpClient) { }

  public CreateTurno(turno:Turno): Observable<Turno> {
    return this.httpClient.post<Turno>('https://localhost:7009/api/turnos',turno)
  }

  public GetTurnos(): Observable<Turno[]> {
    return this.httpClient.get<Turno[]>('https://localhost:7009/api/turnos');
  }

  public GetTurno(id:string): Observable<Turno> {
    return this.httpClient.get<Turno>('https://localhost:7009/api/turnos/' + id);
  }

  public UpdateTurno(turno:Turno): Observable<Turno> {
    return this.httpClient.put<Turno>('https://localhost:7009/api/turnos', turno)
  }

  public DeleteTurno(id:string): Observable<Turno> {
    return this.httpClient.delete<Turno>('https://localhost:7009/api/turnos/' + id);
  }
}
