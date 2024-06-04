import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdenTrabajoPrintComponent } from './orden-trabajo-print.component';

describe('OrdenTrabajoPrintComponent', () => {
  let component: OrdenTrabajoPrintComponent;
  let fixture: ComponentFixture<OrdenTrabajoPrintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrdenTrabajoPrintComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrdenTrabajoPrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
