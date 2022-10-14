import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vehiculo } from 'src/domain/entities';

@Injectable({
  providedIn: 'root'
})
export class VehiculosService {

  constructor(private httpClient:HttpClient) { }

  public CreateVehiculo(vehiculo:Vehiculo): Observable<Vehiculo> {
    return this.httpClient.post<Vehiculo>('https://localhost:7009/api/vehiculos',vehiculo)
  }

  public GetVehiculos(): Observable<Vehiculo[]> {
    return this.httpClient.get<Vehiculo[]>('https://localhost:7009/api/vehiculos');
  }

/*   public GetVehiculo(id:number): Observable<Vehiculo> {
    return this.httpClient.get<Vehiculo>('http://localhost:7009/api/vehiculos/' + id);
  } */

  public UpdateVehiculo(vehiculo:Vehiculo): Observable<Vehiculo> {
    return this.httpClient.put<Vehiculo>('https://localhost:7009/api/vehiculos', vehiculo)
  }

  public DeleteVehiculo(id:string): Observable<Vehiculo> {
    return this.httpClient.delete<Vehiculo>('https://localhost:7009/api/vehiculos/' + id);
  }
}