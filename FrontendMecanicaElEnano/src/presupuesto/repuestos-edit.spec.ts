import { Subject } from 'rxjs';
import { OrdenTrabajoComponent } from '../orden-trabajo/orden-trabajo.component';
import { FormBuilder } from '@angular/forms';
import { PresupuestoComponent } from './presupuesto.component';
import { TrabajoComponent } from '../trabajo/trabajo.component';

for (const entity of ['Presupuesto', 'Trabajo']) {
  describe(entity + ' repuestos editing', () => {
    let component: any;
    const original = { repuestoId: 'original', cantidad: 2, descripcion: 'Filtro', precio: 100, tipo: 0 };
    beforeEach(() => {
      component = entity === 'Presupuesto'
        ? new PresupuestoComponent(null as any, null as any, null as any, new FormBuilder(), null as any, null as any)
        : new TrabajoComponent(null as any, null as any, null as any, null as any, new FormBuilder(), null as any, null as any);
      component['select' + entity]({ repuestos: [original] });
      component['update' + entity + 'Button']();
    });
    it('adds and removes rows and restores saved rows on cancel', () => {
      component.selectedRepuesto = component.dataSourceRepuestos.data[0];
      component.createRepuesto();
      expect(component.selectedRepuesto).toBeNull();
      expect(component.dataSourceRepuestos.data.length).toBe(2);
      component.selectedRepuesto = component.dataSourceRepuestos.data[0];
      component.deleteRepuesto();
      expect(component.dataSourceRepuestos.data.length).toBe(1);
      expect(component.selectedRepuesto).toBeNull();
      component['cancelUpdate' + entity]();
      expect(component.dataSourceRepuestos.data).toEqual([original]);
      expect(component.total).toBe(200);
    });
    it('does not delete another row when the selection is stale', () => {
      component.selectedRepuesto = { ...original };
      component.deleteRepuesto();
      expect(component.dataSourceRepuestos.data.length).toBe(1);
    });
    it('allows adding a row after removing every row', () => {
      component.selectedRepuesto = component.dataSourceRepuestos.data[0];
      component.deleteRepuesto();
      component.createRepuesto();
      expect(component.getFormGroup(0).enabled).toBeTrue();
      expect(component.dataSourceRepuestos.data.length).toBe(1);
    });
  });
}

describe('OrdenTrabajo save before opening parts', () => {
  it('waits for the saved order before navigating to presupuestos', () => {
    const response = new Subject<any>();
    const service = { UpdateOrdenTrabajo: jasmine.createSpy().and.returnValue(response) };
    const router = { navigate: jasmine.createSpy() };
    const component = new OrdenTrabajoComponent(service as any, null as any, null as any, null as any, null as any, router as any);
    const original = { ordenTrabajoId: 'orden', manifiesto: 'Filtro original' } as any;
    component.vehiculo = { vehiculoId: 'vehiculo' } as any;
    component.selectedOrdenTrabajo = original;
    component.dataSource.data = [original];
    component.ordenTrabajoTable = { renderRows: () => {} } as any;
    component.updateOrdenTrabajoButton();
    component.ordenTrabajoForm.patchValue({ manifiesto: 'Filtro nuevo' });
    component.updateOrdenTrabajo(true);
    expect(router.navigate).not.toHaveBeenCalled();
    expect(original.manifiesto).toBe('Filtro original');
    expect(component.state).toBe(2);
    response.next({ ...original, manifiesto: 'Filtro nuevo' });
    expect(router.navigate).toHaveBeenCalledWith(['/presupuestos/ordenTrabajo', 'vehiculo', 'orden']);
    expect(component.selectedOrdenTrabajo!.manifiesto).toBe('Filtro nuevo');
  });
});
