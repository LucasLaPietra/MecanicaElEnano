import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from 'src/environments/environment';
import { Vehiculo } from 'src/domain/entities';

import { VehiculosService } from './vehiculo.service';

describe('VehiculosService', () => {
  let service: VehiculosService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiBaseUrl}/vehiculos`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.inject(VehiculosService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch vehiculos using GET', () => {
    const mockVehiculos = [{ vehiculoId: '1', patente: 'AA111AA' }] as Vehiculo[];

    service.GetVehiculos().subscribe((response) => {
      expect(response).toEqual(mockVehiculos);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockVehiculos);
  });

  it('should create vehiculo using POST', () => {
    const payload = { patente: 'AB123CD' } as Vehiculo;

    service.CreateVehiculo(payload).subscribe((response) => {
      expect(response).toEqual(payload);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(payload);
    req.flush(payload);
  });

  it('should update vehiculo using PUT', () => {
    const payload = { vehiculoId: '1', patente: 'AC456EF' } as Vehiculo;

    service.UpdateVehiculo(payload).subscribe((response) => {
      expect(response).toEqual(payload);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(payload);
    req.flush(payload);
  });

  it('should delete vehiculo using DELETE', () => {
    const vehiculoId = '1';

    service.DeleteVehiculo(vehiculoId).subscribe();

    const req = httpMock.expectOne(`${apiUrl}/${vehiculoId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
