import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateTurno } from 'src/domain/dto';
import { Turno } from 'src/domain/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TurnosService {
  private readonly apiUrl = `${environment.apiBaseUrl}/turnos`;

  constructor(private httpClient:HttpClient) { }

  public CreateTurno(turno:CreateTurno): Observable<Turno> {
    return this.httpClient.post<Turno>(this.apiUrl,turno)
  }

  public GetTurnos(): Observable<Turno[]> {
    return this.httpClient.get<Turno[]>(this.apiUrl);
  }

  public GetTurno(id:string): Observable<Turno> {
    return this.httpClient.get<Turno>(`${this.apiUrl}/${id}`);
  }

  public UpdateTurno(turno:Turno): Observable<Turno> {
    return this.httpClient.put<Turno>(this.apiUrl, turno)
  }

  public DeleteTurno(id:string): Observable<Turno> {
    return this.httpClient.delete<Turno>(`${this.apiUrl}/${id}`);
  }
}
