import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import {
  OrdenTrabajo,
  Repuesto,
  state,
  Vehiculo,
  Presupuesto,
} from 'src/domain/entities';
import { VehiculoModule } from 'src/vehiculo/vehiculo.module';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { OrdenTrabajosService } from './orden-trabajo.service';
import { PresupuestosService } from 'src/presupuesto/presupuesto.service';

@Component({
  selector: 'app-ordenTrabajo',
  templateUrl: './orden-trabajo.component.html',
  styleUrls: ['./orden-trabajo.component.css'],
})
export class OrdenTrabajoComponent implements AfterViewInit {
  vehiculo!: Vehiculo;

  ordenTrabajoForm = new FormGroup({
    fecha: new FormControl(new Date(), Validators.required),
    km: new FormControl('', Validators.required),
    mecanico: new FormControl('', Validators.required),
    manifiesto: new FormControl('', Validators.required),
  });

  displayedColumns: string[] = ['fecha', 'km'];
  selectedOrdenTrabajo: OrdenTrabajo | null = null;
  state: state = 0;
  presupuesto: Presupuesto | undefined;

  @ViewChild(MatTable) ordenTrabajoTable!: MatTable<OrdenTrabajo>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource: MatTableDataSource<OrdenTrabajo>;

  constructor(
    private ordenTrabajoService: OrdenTrabajosService,
    private vehiculoService: VehiculosService,
    private presupuestoService: PresupuestosService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder
  ) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.vehiculoService
      .GetVehiculo(this.route.snapshot.paramMap.get('id')!)
      .subscribe((vehiculo) => (this.vehiculo = vehiculo));
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.vehiculoService
      .GetVehiculo(this.route.snapshot.paramMap.get('id')!)
      .subscribe((vehiculo) => {
        this.vehiculo = vehiculo;
        this.dataSource.data = vehiculo.ordenTrabajos;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.ordenTrabajoForm.disable();
        if (this.route.snapshot.paramMap.get('idOrdenTrabajo')) {
          this.ordenTrabajoService
            .GetOrdenTrabajo(
              this.route.snapshot.paramMap.get('idOrdenTrabajo')!
            )
            .subscribe((t) => {
              this.selectOrdenTrabajo(t);
              this.updateOrdenTrabajoButton();
              this.presupuestoService
                .GetPresupuesto(
                  this.route.snapshot.paramMap.get('idPresupuesto')!
                )
                .subscribe((p) => {
                  this.presupuesto = p;
                  this.updateFormFromPresupuesto(this.presupuesto);
                });
            });
        } else {
          if (this.route.snapshot.paramMap.get('idPresupuesto')) {
            this.createOrdenTrabajoFromPresupuesto();
          }
        }
      });
  }

  selectOrdenTrabajo(row: OrdenTrabajo) {
    if (this.selectedOrdenTrabajo != row) {
      this.cancelUpdateOrdenTrabajo();
      this.selectedOrdenTrabajo = row;
      this.ordenTrabajoForm.patchValue(row);
    } else {
      this.selectedOrdenTrabajo = null;
      this.ordenTrabajoForm.reset();
    }
  }

  compareSelectedtoForm() {
    if (
      this.selectedOrdenTrabajo?.fecha == this.ordenTrabajoForm.value.fecha &&
      this.selectedOrdenTrabajo?.km == this.ordenTrabajoForm.value.km &&
      this.selectedOrdenTrabajo?.manifiesto ==
        this.ordenTrabajoForm.value.manifiesto &&
      this.selectedOrdenTrabajo?.mecanico ==
        this.ordenTrabajoForm.value.mecanico
    )
      return true;
    else return false;
  }

  updateOrdenTrabajoButton() {
    this.state = state.updating;
    this.ordenTrabajoForm.enable();
  }

  cancelUpdateOrdenTrabajo() {
    this.state = state.viewing;
    this.ordenTrabajoForm.patchValue(this.selectedOrdenTrabajo as OrdenTrabajo);
    this.ordenTrabajoForm.disable();
  }

  createOrdenTrabajo() {
    this.ordenTrabajoService
      .CreateOrdenTrabajo(this.route.snapshot.paramMap.get('id')!)
      .subscribe((ordenTrabajo) => {
        this.dataSource.data.push(ordenTrabajo);
        this.dataSource._updateChangeSubscription();
      });
    this.state = state.viewing;
    this.ordenTrabajoForm.reset();
    this.ordenTrabajoForm.disable();
  }

  createOrdenTrabajoFromPresupuesto() {
    this.ordenTrabajoService
      .CreateOrdenTrabajo(this.route.snapshot.paramMap.get('id')!)
      .subscribe((ordenTrabajo) => {
        this.dataSource.data.push(ordenTrabajo);
        this.dataSource._updateChangeSubscription();
        this.selectOrdenTrabajo(ordenTrabajo);
        this.updateOrdenTrabajoButton();
        this.presupuestoService
          .GetPresupuesto(this.route.snapshot.paramMap.get('idPresupuesto')!)
          .subscribe((p) => {
            this.presupuesto = p;
            this.updateFormFromPresupuesto(this.presupuesto);
          });
      });
  }

  updateFormFromPresupuesto(presupuesto: Presupuesto) {
    let manifiestoValue = `${this.ordenTrabajoForm.value.manifiesto}\n`;
    presupuesto.repuestos.forEach((repuesto) => {
      manifiestoValue += `- ${repuesto.descripcion}\n`;
    });
    this.ordenTrabajoForm.patchValue({
      km: presupuesto.km,
      manifiesto: manifiestoValue,
    });
  }

  updateOrdenTrabajo() {
    if (this.selectedOrdenTrabajo) {
      const indexOfObject = this.dataSource.data.indexOf(
        this.selectedOrdenTrabajo
      );
      let ordenTrabajoActualizado: OrdenTrabajo = this.selectedOrdenTrabajo;
      ordenTrabajoActualizado.fecha = this.ordenTrabajoForm.value.fecha as Date;
      ordenTrabajoActualizado.km = this.ordenTrabajoForm.value.km as string;
      ordenTrabajoActualizado.manifiesto = this.ordenTrabajoForm.value
        .manifiesto as string;
      ordenTrabajoActualizado.mecanico = this.ordenTrabajoForm.value
        .mecanico as string;
      this.ordenTrabajoService
        .UpdateOrdenTrabajo(ordenTrabajoActualizado)
        .subscribe((ordenTrabajo) => {
          this.dataSource.data[indexOfObject] = ordenTrabajo;
        });
      this.ordenTrabajoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state = state.viewing;
      this.ordenTrabajoForm.disable();
    }
  }

  deleteOrdenTrabajo() {
    if (this.selectedOrdenTrabajo) {
      this.ordenTrabajoService
        .DeleteOrdenTrabajo(this.selectedOrdenTrabajo.ordenTrabajoId)
        .subscribe();
      this.dataSource.data = this.dataSource.data.filter(
        (h) => h !== this.selectedOrdenTrabajo
      );
      this.ordenTrabajoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.selectedOrdenTrabajo = null;
      this.ordenTrabajoForm.reset();
    }
  }

  printOrdenTrabajo() {}

  getFormattedDate(date: Date) {
    return date.getDate();
  }
}
