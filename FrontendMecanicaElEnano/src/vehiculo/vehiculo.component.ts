import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { state, Vehiculo } from 'src/domain/entities';

@Component({
  selector: 'app-vehiculo',
  templateUrl: './vehiculo.component.html',
  styleUrls: ['./vehiculo.component.css']
})
export class VehiculoComponent implements AfterViewInit {

  vehiculos : Vehiculo[] =
  [
    {
      cliente: 'Lucas',
      cuit: '2039760439',
      direccion:'san martin 977',
      mail:'lelapietra@gmail.com',
      marcaYModelo:'Chevrolet Cruze',
      nroChasis:'1111',
      nroMotor:'1111',
      patente:'AA111AA',
      telefono:'3442456149'
    },
    {
      cliente: 'John',
      cuit: '111111111',
      direccion:'direccion 1',
      mail:'john@gmail.com',
      marcaYModelo:'VW Gol',
      nroChasis:'1111',
      nroMotor:'1111',
      patente:'AA111AA',
      telefono:'1111111111'
    },
    {
      cliente: 'Susana',
      cuit: '111111111',
      direccion:'direccion 2',
      mail:'susan@gmail.com',
      marcaYModelo:'Ford Fiesta',
      nroChasis:'1111',
      nroMotor:'1111',
      patente:'AA111AA',
      telefono:'111111111'
    }
  ]
  
  vehiculoForm = new FormGroup({
    patente: new FormControl('', [Validators.required, Validators.maxLength(7)]),
    cliente: new FormControl('', Validators.required),
    marcaYModelo: new FormControl('', Validators.required),
    direccion: new FormControl('', Validators.required),
    telefono: new FormControl('', Validators.required),
    mail: new FormControl('', [Validators.required, Validators.email]),
    nroMotor: new FormControl('', [Validators.required, Validators.maxLength(17)]),
    nroChasis: new FormControl('', [Validators.required, Validators.maxLength(17)]),
    cuit: new FormControl('', [Validators.required, Validators.maxLength(11)]),
  });

  displayedColumns: string[] = ['patente', 'marcaYModelo', 'cliente', 'telefono'];
  dataSource: MatTableDataSource<Vehiculo>;
  selectedVehicle: Vehiculo|null = null;
  state: state = 0;

  @ViewChild(MatTable) vehiculoTable!: MatTable<Vehiculo>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  constructor() {
    this.dataSource = new MatTableDataSource(this.vehiculos);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.vehiculoForm.disable();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  selectVehicle(row: Vehiculo){
    if (this.selectedVehicle!=row)
    {
      this.selectedVehicle=row;
      this.vehiculoForm.patchValue(row);
    }
    else 
    {
      this.selectedVehicle = null;
      this.vehiculoForm.reset();
    }
  }

  compareSelectedtoForm(){
    if (this.selectedVehicle?.cliente==this.vehiculoForm.value.cliente &&
      this.selectedVehicle?.cuit==this.vehiculoForm.value.cuit &&
      this.selectedVehicle?.direccion==this.vehiculoForm.value.direccion &&
      this.selectedVehicle?.mail==this.vehiculoForm.value.mail &&
      this.selectedVehicle?.marcaYModelo==this.vehiculoForm.value.marcaYModelo &&
      this.selectedVehicle?.nroChasis==this.vehiculoForm.value.nroChasis &&
      this.selectedVehicle?.nroMotor==this.vehiculoForm.value.nroMotor &&
      this.selectedVehicle?.patente==this.vehiculoForm.value.patente &&
      this.selectedVehicle?.telefono==this.vehiculoForm.value.telefono)
      return true; else return false
  }

  updateVehicleButton(){
    this.state=state.updating;
    this.vehiculoForm.enable();
  }

  cancelUpdateVehicle(){
    this.state=state.viewing;
    this.vehiculoForm.patchValue(this.selectedVehicle as Vehiculo);
    this.vehiculoForm.disable();
  }

  updateVehicle(){
    const indexOfObject =  this.dataSource.data.indexOf(this.selectedVehicle as Vehiculo);
    this.dataSource.data[indexOfObject]=this.vehiculoForm.value as Vehiculo;
    this.vehiculoTable.renderRows();
    this.dataSource._updateChangeSubscription();
    this.state=state.viewing;
    this.vehiculoForm.disable();
  }

  createVehicleButton(){
    this.state=state.creating;
    this.selectedVehicle = null;
    this.vehiculoForm.reset();
    this.vehiculoForm.enable();
  }

  cancelCreateVehicle(){
    this.state=state.viewing;
    this.vehiculoForm.reset();
    this.vehiculoForm.disable();
  }

  createVehicle(){
    const vehiculoACrear: Vehiculo = this.vehiculoForm.value as Vehiculo;
    this.dataSource.data.push(vehiculoACrear);
    this.vehiculoTable.renderRows();
    this.dataSource._updateChangeSubscription();
    console.log(this.dataSource.data);
    this.state=state.viewing;
    this.vehiculoForm.reset();
    this.vehiculoForm.disable();
  }

  deleteVehicle(){
    const indexOfObject =  this.dataSource.data.indexOf(this.selectedVehicle as Vehiculo);
    this.dataSource.data.splice(indexOfObject, 1);
    this.vehiculoTable.renderRows();
    this.dataSource._updateChangeSubscription();
    this.selectedVehicle = null;
    this.vehiculoForm.reset();
    console.log(this.dataSource.data);
  }

  openWhatsapp(telephone: string){
    const url : string= "https://api.whatsapp.com/send?phone=" + telephone;
    window.open(url, "_blank");
  }
}
