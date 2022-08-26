import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Vehiculo } from 'src/domain/entities';

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
    patente: new FormControl('', Validators.maxLength(7)),
    cliente: new FormControl('', Validators.minLength(2)),
    marcaYModelo: new FormControl('', Validators.minLength(2)),
    direccion: new FormControl('', Validators.minLength(2)),
    telefono: new FormControl('', Validators.minLength(2)),
    mail: new FormControl('', Validators.minLength(2)),
    nroMotor: new FormControl('', Validators.minLength(2)),
    nroChasis: new FormControl('', Validators.minLength(2)),
    cuit: new FormControl('', Validators.minLength(2)),
  });

  displayedColumns: string[] = ['patente', 'modelo', 'cliente', 'telefono'];
  dataSource: MatTableDataSource<Vehiculo>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  constructor() {
    this.dataSource = new MatTableDataSource(this.vehiculos);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
