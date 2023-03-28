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

  manoObra: number = 0;
  repuestos: number = 0;
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
      this.dataSourceRepuestos = new MatTableDataSource();
      this.dataSourceRepuestos = new MatTableDataSource((this.repuestoForm.get('repuestos') as FormArray).controls);
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
  }

  selectPresupuesto(row: Presupuesto){
    if (this.selectedPresupuesto!=row)
    {
      this.selectedPresupuesto=row;
      this.presupuestoForm.patchValue(row);
      this.dataSourceRepuestos.data = this.selectedPresupuesto.repuestos;
      this.calculateCosts(this.selectedPresupuesto);
    }
    else 
    {
      this.selectedPresupuesto = null;
      this.dataSourceRepuestos.data = [];
      this.presupuestoForm.reset();
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

  calculateCosts(presupuesto: Presupuesto){
    this.manoObra = 0;
    this.repuestos = 0;
    presupuesto.repuestos.forEach(r => {
      if (r.tipo=0)
      {
        this.repuestos += r.precio;
      }
      else
      {
        this.manoObra += r.precio;
      }
    });
    this.total = this.manoObra + this.repuestos;
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
  }

  cancelUpdatePresupuesto(){
    this.state=state.viewing;
    this.presupuestoForm.patchValue(this.selectedPresupuesto as Presupuesto);
    this.presupuestoForm.disable();
  }

  createPresupuesto(){
    this.presupuestoService.CreatePresupuesto(this.route.snapshot.paramMap.get('id')!)
    .subscribe(presupuesto => {
      this.dataSource.data.push(presupuesto);
      this.dataSource._updateChangeSubscription();
    });
    console.log(this.dataSource.data);
    this.state=state.viewing;
    this.presupuestoForm.reset();
    this.presupuestoForm.disable();
  }

  updatePresupuesto(){
    if(this.selectedPresupuesto){
      const indexOfObject =  this.dataSource.data.indexOf(this.selectedPresupuesto);
      console.log(this.presupuestoForm.value as Presupuesto)
      let presupuestoActualizado: Presupuesto = this.selectedPresupuesto;
      presupuestoActualizado.fecha = this.presupuestoForm.value.fecha as Date;
      presupuestoActualizado.validoHasta = this.presupuestoForm.value.validoHasta as Date;
      presupuestoActualizado.km = this.presupuestoForm.value.km as string;
      presupuestoActualizado.trabajoARealizar = this.presupuestoForm.value.trabajoARealizar as string;
      this.presupuestoService.UpdatePresupuesto(presupuestoActualizado).subscribe(presupuesto => {
        this.dataSource.data[indexOfObject] = presupuesto;
      });
      this.presupuestoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state=state.viewing;
      this.presupuestoForm.disable();
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

  createRepuestosForm() {
    let listaVacia: Repuesto[] = []
    this.repuestoForm = this.formBuilder.group({
      repuestos: this.formBuilder.array(listaVacia),
    });
  }

  get formArr() {
    return this.repuestoForm.get('repuestos') as FormArray;
  }

  addRow(repuesto: Repuesto) {
    return this.formBuilder.group({
      cantidad: [repuesto.cantidad],
      descripcion: [repuesto.descripcion],
      precio: [repuesto.precio],
      tipo: [repuesto.tipo],
    });
  }

  addNewRow() {
    let repuesto: Repuesto = {
      cantidad: 0,
      descripcion: "",
      precio: 0,
      tipo: 0,
    };
    this.formArr.push(this.addRow(repuesto));
  }

  createRepuesto(){
    this.addNewRow();
  }

  deleteRepuesto(){
    if(this.selectedRepuesto){
      let index = this.selectedPresupuesto?.repuestos.indexOf(this.selectedRepuesto)
      if(index){
        this.formArr.removeAt(index)
        this.selectedRepuesto = null;
      }
    }
  }

  getFormattedDate(date:Date){
    console.log(date.toString())
    console.log(new Date(parseInt(/-?\d+/.exec(date.toString())![0])))
    return date.getDate();
  }


}
