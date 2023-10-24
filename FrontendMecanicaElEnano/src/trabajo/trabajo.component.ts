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
  Trabajo,
  Repuesto,
  state,
  Vehiculo,
  Presupuesto,
} from 'src/domain/entities';
import { VehiculoModule } from 'src/vehiculo/vehiculo.module';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { TrabajosService } from './trabajo.service';
import { PresupuestosService } from 'src/presupuesto/presupuesto.service';
import { MatDialog } from '@angular/material/dialog';
import { CancelModalComponent } from 'src/cancel-modal/cancel-modal.component';

@Component({
  selector: 'app-trabajo',
  templateUrl: './trabajo.component.html',
  styleUrls: ['./trabajo.component.css'],
})
export class TrabajoComponent implements AfterViewInit {
  vehiculo!: Vehiculo;

  repuestoForm!: FormGroup;

  trabajoForm = new FormGroup({
    fecha: new FormControl(new Date(), Validators.required),
    km: new FormControl('', Validators.required),
    trabajosRealizados: new FormControl('', Validators.required),
    trabajosPendientes: new FormControl('', Validators.required),
  });

  displayedColumns: string[] = ['fecha', 'km'];
  displayedColumnsRepuestos: string[] = [
    'cantidad',
    'descripcion',
    'tipoTrabajo',
    'precio',
  ];
  selectedTrabajo: Trabajo | null = null;
  selectedRepuesto: Repuesto | null = null;
  state: state = 0;
  presupuesto: Presupuesto | undefined;

  tipoOptions = [
    { value: 0, label: 'Repuesto' },
    { value: 1, label: 'Mano obra' },
  ];
  manoObraPrice: number = 0;
  repuestosPrice: number = 0;
  total: number = 0;

  @ViewChild(MatTable) trabajoTable!: MatTable<Trabajo>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource: MatTableDataSource<Trabajo>;
  dataSourceRepuestos: MatTableDataSource<any>;

  constructor(
    private trabajoService: TrabajosService,
    private vehiculoService: VehiculosService,
    private presupuestoService: PresupuestosService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialog: MatDialog
  ) {
    this.dataSource = new MatTableDataSource();
    this.createRepuestosForm();
    this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(
      this.repuestoForm.controls['repuestos'].value
    );
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
        this.dataSource.data = vehiculo.trabajos;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
    this.trabajoForm.disable();
    this.repuestoForm.disable();
    if (this.route.snapshot.paramMap.get('idTrabajo')) {
      this.trabajoService
        .GetTrabajo(this.route.snapshot.paramMap.get('idTrabajo')!)
        .subscribe((t) => {
          this.selectTrabajo(t);
          this.updateTrabajoButton();
          this.presupuestoService
            .GetPresupuesto(this.route.snapshot.paramMap.get('idPresupuesto')!)
            .subscribe((p) => {
              this.presupuesto = p;
              const repuestos = this.repuestoForm.get('repuestos') as FormArray;
              this.presupuesto!.repuestos.forEach((repuesto) => {
                repuestos.push(this.addRow(repuesto));
              });
              this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(
                this.repuestoForm.controls['repuestos'].value
              );
            });
          console.log(this.presupuesto);
        });
    }
  }

  selectTrabajo(row: Trabajo) {
    if (this.selectedTrabajo != row) {
      this.cancelUpdateTrabajo();
      this.createRepuestosForm();
      this.selectedTrabajo = row;
      this.trabajoForm.patchValue(row);
      const repuestos = this.repuestoForm.get('repuestos') as FormArray;
      repuestos.valueChanges.subscribe((value) => {
        this.calculateCosts();
      });
      this.selectedTrabajo.repuestos.forEach((repuesto) => {
        repuestos.push(this.addRow(repuesto));
      });
      this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(
        this.repuestoForm.controls['repuestos'].value
      );
      this.repuestoForm.disable();
      this.calculateCosts();
    } else {
      this.selectedTrabajo = null;
      this.dataSourceRepuestos.data = [];
      this.trabajoForm.reset();
      this.createRepuestosForm();
      this.calculateCosts();
    }
  }

  selectRepuesto(row: Repuesto) {
    if (this.selectedRepuesto != row) {
      this.selectedRepuesto = row;
    } else {
      this.selectedRepuesto = null;
    }
  }

  calculateCosts() {
    this.manoObraPrice = 0;
    this.repuestosPrice = 0;
    if (this.selectedTrabajo) {
      this.repuestoForm.get('repuestos')?.value.forEach((r: Repuesto) => {
        if (r.tipo == 0) {
          this.repuestosPrice += r.precio * r.cantidad;
        } else {
          this.manoObraPrice += r.precio * r.cantidad;
        }
      });
    }
    this.total = this.manoObraPrice + this.repuestosPrice;
  }

  compareSelectedtoForm() {
    if (
      this.selectedTrabajo?.fecha == this.trabajoForm.value.fecha &&
      this.selectedTrabajo?.km == this.trabajoForm.value.km &&
      this.selectedTrabajo?.trabajosPendientes ==
        this.trabajoForm.value.trabajosPendientes &&
      this.selectedTrabajo?.trabajosRealizados ==
        this.trabajoForm.value.trabajosRealizados
    )
      return true;
    else return false;
  }

  updateTrabajoButton() {
    this.state = state.updating;
    this.trabajoForm.enable();
    this.repuestoForm.enable();
  }

  cancelUpdateTrabajo() {
    this.state = state.viewing;
    this.trabajoForm.patchValue(this.selectedTrabajo as Trabajo);
    this.trabajoForm.disable();
    this.repuestoForm.disable();
  }

  createTrabajo() {
    this.trabajoService
      .CreateTrabajo(this.route.snapshot.paramMap.get('id')!)
      .subscribe((trabajo) => {
        this.dataSource.data.push(trabajo);
        this.dataSource._updateChangeSubscription();
      });
    this.state = state.viewing;
    this.trabajoForm.reset();
    this.trabajoForm.disable();
  }

  updateTrabajo() {
    if (this.selectedTrabajo) {
      const indexOfObject = this.dataSource.data.indexOf(this.selectedTrabajo);
      let trabajoActualizado: Trabajo = this.selectedTrabajo;
      trabajoActualizado.fecha = this.trabajoForm.value.fecha as Date;
      trabajoActualizado.km = this.trabajoForm.value.km as string;
      trabajoActualizado.trabajosPendientes = this.trabajoForm.value
        .trabajosPendientes as string;
      trabajoActualizado.trabajosRealizados = this.trabajoForm.value
        .trabajosRealizados as string;
      trabajoActualizado.repuestos =
        this.repuestoForm.controls['repuestos'].value;
      this.trabajoService
        .UpdateTrabajo(trabajoActualizado)
        .subscribe((trabajo) => {
          this.dataSource.data[indexOfObject] = trabajo;
        });
      this.trabajoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state = state.viewing;
      this.trabajoForm.disable();
      this.repuestoForm.disable();
    }
  }

  deleteTrabajo() {
    if (this.selectedTrabajo) {
      this.trabajoService
        .DeleteTrabajo(this.selectedTrabajo.trabajoId)
        .subscribe();
      this.dataSource.data = this.dataSource.data.filter(
        (h) => h !== this.selectedTrabajo
      );
      this.trabajoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.selectedTrabajo = null;
      this.trabajoForm.reset();
    }
  }

  printTrabajo() {}

  createRepuestosForm() {
    this.repuestoForm = this.formBuilder.group({
      repuestos: this.formBuilder.array([]),
    });
  }

  addRow(repuesto: Repuesto) {
    return this.formBuilder.group({
      repuestoId: [repuesto.repuestoId],
      cantidad: [repuesto.cantidad],
      descripcion: [repuesto.descripcion],
      precio: [repuesto.precio],
      tipo: [repuesto.tipo],
    });
  }

  createRepuesto() {
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    repuestos.push(
      this.formBuilder.group({
        repuestoId: [null],
        cantidad: [1, Validators.required],
        descripcion: ['', Validators.required],
        precio: [0, Validators.required],
        tipo: [0, Validators.required],
      })
    );
    this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(
      this.repuestoForm.controls['repuestos'].value
    );
  }

  deleteRepuesto() {
    const index = this.dataSourceRepuestos.data.indexOf(this.selectedRepuesto);
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    repuestos.removeAt(index);
    this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(
      this.repuestoForm.controls['repuestos'].value
    );
  }

  getFormGroup(index: number): FormGroup {
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    return repuestos.controls[index] as FormGroup;
  }

  getFormattedDate(date: Date) {
    return date.getDate();
  }

  openUpdateDialog(): void {
    this.dialog
      .open(CancelModalComponent, {
        data: {action:'update',entity:'trabajo'}
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.updateTrabajo();
        }
      }).unsubscribe();
    }

    openDeleteDialog(): void {
      this.dialog
        .open(CancelModalComponent, {
          data: {action:'delete',entity:'trabajo'}
        })
        .afterClosed()
        .subscribe((confirmado: Boolean) => {
          if (confirmado) {
            this.deleteTrabajo();
          }
        }).unsubscribe();
      }
}
