import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrabajoPrintComponent } from './trabajo-print.component';

describe('TrabajoPrintComponent', () => {
  let component: TrabajoPrintComponent;
  let fixture: ComponentFixture<TrabajoPrintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrabajoPrintComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrabajoPrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
