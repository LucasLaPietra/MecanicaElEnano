import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PresupuestoPrintComponent } from './presupuesto-print.component';

describe('PresupuestoPrintComponent', () => {
  let component: PresupuestoPrintComponent;
  let fixture: ComponentFixture<PresupuestoPrintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PresupuestoPrintComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PresupuestoPrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
