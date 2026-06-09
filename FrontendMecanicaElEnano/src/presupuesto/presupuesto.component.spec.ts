import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { PresupuestoComponent } from './presupuesto.component';
import { PresupuestosService } from './presupuesto.service';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';

describe('PresupuestoComponent', () => {
  let component: PresupuestoComponent;
  let fixture: ComponentFixture<PresupuestoComponent>;

  const presupuestoServiceMock = {
    CreatePresupuesto: jasmine.createSpy().and.returnValue(of({})),
    UpdatePresupuesto: jasmine.createSpy().and.returnValue(of({})),
    DeletePresupuesto: jasmine.createSpy().and.returnValue(of({})),
  };

  const vehiculoServiceMock = {
    GetVehiculo: jasmine.createSpy().and.returnValue(of({ presupuestos: [] })),
  };

  const activatedRouteMock = {
    snapshot: {
      paramMap: {
        get: (_key: string) => null,
      },
    },
  };

  const dialogMock = {
    open: jasmine.createSpy(),
  };

  const routerMock = {
    createUrlTree: jasmine.createSpy(),
    serializeUrl: jasmine.createSpy(),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PresupuestoComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: PresupuestosService, useValue: presupuestoServiceMock },
        { provide: VehiculosService, useValue: vehiculoServiceMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock },
        { provide: MatDialog, useValue: dialogMock },
        { provide: Router, useValue: routerMock },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PresupuestoComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add one repuesto row with defaults', () => {
    component.createRepuesto();

    expect(component.dataSourceRepuestos.data.length).toBe(1);
    expect(component.dataSourceRepuestos.data[0].cantidad).toBe(1);
    expect(component.dataSourceRepuestos.data[0].precio).toBe(0);
  });

  it('should remove selected repuesto row', () => {
    component.createRepuesto();
    component.createRepuesto();
    component.selectedRepuesto = component.dataSourceRepuestos.data[0];

    component.deleteRepuesto();

    expect(component.dataSourceRepuestos.data.length).toBe(1);
  });

  it('should calculate mano de obra and repuestos totals', () => {
    component.selectedPresupuesto = {} as any;
    const repuestos = component.repuestoForm.get('repuestos') as any;
    repuestos.push(component.addRow({
      repuestoId: '1',
      cantidad: 2,
      descripcion: 'Filtro',
      precio: 100,
      tipo: 0,
    } as any));
    repuestos.push(component.addRow({
      repuestoId: '2',
      cantidad: 3,
      descripcion: 'Mano de obra',
      precio: 50,
      tipo: 1,
    } as any));

    component.calculateCosts();

    expect(component.repuestosPrice).toBe(200);
    expect(component.manoObraPrice).toBe(150);
    expect(component.total).toBe(350);
  });
});
