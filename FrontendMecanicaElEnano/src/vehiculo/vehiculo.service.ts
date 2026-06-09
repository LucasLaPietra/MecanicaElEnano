import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vehiculo } from 'src/domain/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VehiculosService {
  private readonly apiUrl = `${environment.apiBaseUrl}/vehiculos`;

  constructor(private httpClient:HttpClient) { }

  public CreateVehiculo(vehiculo:Vehiculo): Observable<Vehiculo> {
    return this.httpClient.post<Vehiculo>(this.apiUrl,vehiculo)
  }

  public GetVehiculos(): Observable<Vehiculo[]> {
    return this.httpClient.get<Vehiculo[]>(this.apiUrl);
  }

  public GetVehiculo(id:string): Observable<Vehiculo> {
    return this.httpClient.get<Vehiculo>(`${this.apiUrl}/${id}`);
  } 

  public UpdateVehiculo(vehiculo:Vehiculo): Observable<Vehiculo> {
    return this.httpClient.put<Vehiculo>(this.apiUrl, vehiculo)
  }

  public DeleteVehiculo(id:string): Observable<Vehiculo> {
    return this.httpClient.delete<Vehiculo>(`${this.apiUrl}/${id}`);
  }
}