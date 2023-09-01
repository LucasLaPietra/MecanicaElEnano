import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Presupuesto, Repuesto, state, Vehiculo } from 'src/domain/entities';
import { VehiculoModule } from 'src/vehiculo/vehiculo.module';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { PresupuestosService } from './presupuesto.service';

@Component({
  selector: 'app-presupuesto',
  templateUrl: './presupuesto.component.html',
  styleUrls: ['./presupuesto.component.css']
})
export class PresupuestoComponent implements AfterViewInit {

  vehiculo!: Vehiculo;

  repuestoForm!: FormGroup;

  trabajoId: String | undefined;

  presupuestoForm = new FormGroup({
    fecha: new FormControl(new Date(), Validators.required),
    validoHasta: new FormControl(new Date(), Validators.required),
    km: new FormControl('', Validators.required),
    trabajoARealizar: new FormControl('', Validators.required),
  });

  displayedColumns: string[] = ['fecha', 'validoHasta', 'km'];
  displayedColumnsRepuestos: string[] = ['cantidad', 'descripcion', 'tipoTrabajo', 'precio'];
  selectedPresupuesto: Presupuesto|null = null;
  selectedRepuesto: Repuesto|null = null;
  state: state = 0;

  tipoOptions = [
    { value: 0, label: 'Repuesto' },
    { value: 1, label: 'Mano obra' },
  ];
  manoObraPrice: number = 0;
  repuestosPrice: number = 0;
  total: number = 0;

  @ViewChild(MatTable) presupuestoTable!: MatTable<Presupuesto>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource: MatTableDataSource<Presupuesto>;
  dataSourceRepuestos: MatTableDataSource<any>;

  constructor(
    private presupuestoService: PresupuestosService,
    private vehiculoService: VehiculosService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder) {
      this.dataSource = new MatTableDataSource();
      this.createRepuestosForm();
      this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(this.repuestoForm.controls['repuestos'].value);
     }

  ngOnInit(): void {
    this.vehiculoService.GetVehiculo(this.route.snapshot.paramMap.get('id')!)
    .subscribe(vehiculo=>this.vehiculo=vehiculo)
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.vehiculoService.GetVehiculo(this.route.snapshot.paramMap.get('id')!)
    .subscribe(vehiculo=>{
      this.vehiculo=vehiculo;
      this.dataSource.data = vehiculo.presupuestos;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    })
    this.presupuestoForm.disable();
    this.repuestoForm.disable();
    if (this.route.snapshot.paramMap.get('idTrabajo')){
      this.trabajoId = this.route.snapshot.paramMap.get('idTrabajo')!;
    }
  }

  selectPresupuesto(row: Presupuesto){
    if (this.selectedPresupuesto!=row)
    {
      this.cancelUpdatePresupuesto()
      this.createRepuestosForm();
      this.selectedPresupuesto=row;
      this.presupuestoForm.patchValue(row);
      const repuestos = this.repuestoForm.get('repuestos') as FormArray;
      repuestos.valueChanges.subscribe((value) => {
        this.calculateCosts();
      });
      this.selectedPresupuesto.repuestos.forEach(repuesto => {
        repuestos.push(
          this.addRow(repuesto)
        );
      });
      this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(this.repuestoForm.controls['repuestos'].value);
      this.repuestoForm.disable()
      this.calculateCosts()
    }
    else
    {
      this.selectedPresupuesto = null;
      this.dataSourceRepuestos.data = [];
      this.presupuestoForm.reset();
      this.createRepuestosForm();
      this.calculateCosts()
    }
  }

  selectRepuesto(row: Repuesto){
    if (this.selectedRepuesto!=row)
    {
      this.selectedRepuesto=row;
    }
    else
    {
      this.selectedRepuesto = null;
    }
  }

  calculateCosts(){
    this.manoObraPrice = 0;
    this.repuestosPrice = 0;
    if(this.selectedPresupuesto){
      this.repuestoForm.get('repuestos')?.value.forEach((r: Repuesto) => {
        if (r.tipo==0)
        {
          this.repuestosPrice += r.precio * r.cantidad;
        }
        else
        {
          this.manoObraPrice += r.precio * r.cantidad;
        }
      });
    }
    this.total = this.manoObraPrice + this.repuestosPrice;
  }

  compareSelectedtoForm(){
    if (this.selectedPresupuesto?.fecha==this.presupuestoForm.value.fecha &&
      this.selectedPresupuesto?.validoHasta==this.presupuestoForm.value.validoHasta &&
      this.selectedPresupuesto?.km==this.presupuestoForm.value.km &&
      this.selectedPresupuesto?.trabajoARealizar==this.presupuestoForm.value.trabajoARealizar)
      return true; else return false
  }

  updatePresupuestoButton(){
    this.state=state.updating;
    this.presupuestoForm.enable();
    this.repuestoForm.enable()
  }

  cancelUpdatePresupuesto(){
    this.state=state.viewing;
    this.presupuestoForm.patchValue(this.selectedPresupuesto as Presupuesto);
    this.presupuestoForm.disable();
    this.repuestoForm.disable()
  }

  createPresupuesto(){
    this.presupuestoService.CreatePresupuesto(this.route.snapshot.paramMap.get('id')!)
    .subscribe(presupuesto => {
      this.dataSource.data.push(presupuesto);
      this.dataSource._updateChangeSubscription();
    });
    this.state=state.viewing;
    this.presupuestoForm.reset();
    this.presupuestoForm.disable();
  }

  updatePresupuesto(){
    if(this.selectedPresupuesto){
      const indexOfObject =  this.dataSource.data.indexOf(this.selectedPresupuesto);
      let presupuestoActualizado: Presupuesto = this.selectedPresupuesto;
      presupuestoActualizado.fecha = this.presupuestoForm.value.fecha as Date;
      presupuestoActualizado.validoHasta = this.presupuestoForm.value.validoHasta as Date;
      presupuestoActualizado.km = this.presupuestoForm.value.km as string;
      presupuestoActualizado.trabajoARealizar = this.presupuestoForm.value.trabajoARealizar as string;
      presupuestoActualizado.repuestos= this.repuestoForm.controls['repuestos'].value
      this.presupuestoService.UpdatePresupuesto(presupuestoActualizado).subscribe(presupuesto => {
        this.dataSource.data[indexOfObject] = presupuesto;
      });
      this.presupuestoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state=state.viewing;
      this.presupuestoForm.disable();
      this.repuestoForm.disable();
    }
  }

  deletePresupuesto(){
    if(this.selectedPresupuesto){
      this.presupuestoService.DeletePresupuesto(this.selectedPresupuesto.presupuestoId).subscribe();
      this.dataSource.data = this.dataSource.data.filter(h => h !== this.selectedPresupuesto);
      this.presupuestoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.selectedPresupuesto = null;
      this.presupuestoForm.reset();
    }
  }

  printPresupuesto(){

  }

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

  createRepuesto(){
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    repuestos.push(
      this.formBuilder.group({
        repuestoId: [null],
        cantidad: [1, Validators.required],
        descripcion: ["", Validators.required],
        precio: [0, Validators.required],
        tipo: [0, Validators.required],
      })
    );
    this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(this.repuestoForm.controls['repuestos'].value);
  }

  deleteRepuesto() {
    const index = this.dataSourceRepuestos.data.indexOf(this.selectedRepuesto);
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    repuestos.removeAt(index);
    this.dataSourceRepuestos = new MatTableDataSource<Repuesto>(this.repuestoForm.controls['repuestos'].value);
  }

  getFormGroup(index: number): FormGroup {
    const repuestos = this.repuestoForm.get('repuestos') as FormArray;
    return repuestos.controls[index] as FormGroup;
  }

  getFormattedDate(date:Date){
    return date.getDate();
  }


}
