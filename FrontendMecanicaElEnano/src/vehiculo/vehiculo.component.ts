import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { state, Turno, Vehiculo } from 'src/domain/entities';
import { VehiculosService } from './vehiculo.service';
import { ActivatedRoute } from '@angular/router';
import { CancelModalComponent } from 'src/cancel-modal/cancel-modal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-vehiculo',
  templateUrl: './vehiculo.component.html',
  styleUrls: ['./vehiculo.component.css']
})
export class VehiculoComponent implements AfterViewInit {

  vehiculos : Vehiculo[] = [];
  date: string | undefined;

  vehiculoForm = new FormGroup({
    patente: new FormControl('', [Validators.required, Validators.maxLength(7)]),
    cliente: new FormControl('', Validators.required),
    modelo: new FormControl('', Validators.required),
    direccion: new FormControl('', Validators.required),
    telefono: new FormControl('', Validators.required),
    mail: new FormControl('', [Validators.required, Validators.email]),
    numeroMotor: new FormControl('', [Validators.required, Validators.maxLength(17)]),
    numeroChasis: new FormControl('', [Validators.required, Validators.maxLength(17)]),
    cuit: new FormControl('', [Validators.required, Validators.maxLength(11)]),
  });

  displayedColumns: string[] = ['patente', 'marcaYModelo', 'cliente', 'telefono'];
  dataSource: MatTableDataSource<Vehiculo>;
  selectedVehicle: Vehiculo|null = null;
  state: state = 0;

  @ViewChild(MatTable) vehiculoTable!: MatTable<Vehiculo>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private vehiculoService: VehiculosService,
              private route: ActivatedRoute,
              public dialog: MatDialog) {
    this.dataSource = new MatTableDataSource();
  }

  ngAfterViewInit() {
    if (this.route.snapshot.params['date']) {
      this.date = this.route.snapshot.paramMap.get('date')!
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.vehiculoService.GetVehiculos().subscribe(vehiculos => {
      this.dataSource.data = vehiculos;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.vehiculoForm.disable();
  }

  getVehiculos(): void {
    this.vehiculoService.GetVehiculos()
    .subscribe(vehiculos => this.vehiculos = vehiculos);
    this.vehiculoTable.renderRows();
    this.dataSource._updateChangeSubscription();
    console.log(this.dataSource.data);
    console.log(this.vehiculos);
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
      this.selectedVehicle?.modelo==this.vehiculoForm.value.modelo &&
      this.selectedVehicle?.numeroChasis==this.vehiculoForm.value.numeroChasis &&
      this.selectedVehicle?.nroMotor==this.vehiculoForm.value.numeroMotor &&
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
    this.vehiculoService.CreateVehiculo(vehiculoACrear)
    .subscribe(vehiculo => {
      this.dataSource.data.push(vehiculo);
      this.dataSource._updateChangeSubscription();
      this.vehiculoTable.renderRows();
      this.state=state.viewing;
      this.vehiculoForm.reset();
      this.vehiculoForm.disable();
    });
  }

  updateVehicle(){
    if(this.selectedVehicle){
      const indexOfObject =  this.dataSource.data.indexOf(this.selectedVehicle);
      this.vehiculoService.UpdateVehiculo(this.selectedVehicle).subscribe(vehiculo => {
        this.dataSource.data[indexOfObject] = vehiculo;
      });
      this.vehiculoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state=state.viewing;
      this.vehiculoForm.disable();
    }
  }

  deleteVehicle(){
    if(this.selectedVehicle){
      this.vehiculoService.DeleteVehiculo(this.selectedVehicle.vehiculoId).subscribe();
      this.dataSource.data = this.dataSource.data.filter(h => h !== this.selectedVehicle);
      this.vehiculoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.selectedVehicle = null;
      this.vehiculoForm.reset();
    }
  }

  openWhatsapp(telephone: string){
    const url : string= "https://api.whatsapp.com/send?phone=" + telephone;
    window.open(url, "_blank");
  }

  openUpdateDialog(): void {
    this.dialog
      .open(CancelModalComponent, {
        data: {action:'modificar',entity:'vehiculo'}
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.updateVehicle();
        }
      })
    }

    openDeleteDialog(): void {
      this.dialog
        .open(CancelModalComponent, {
          data: {action:'borrar',entity:'vehiculo'}
        })
        .afterClosed()
        .subscribe((confirmado: Boolean) => {
          if (confirmado) {
            this.deleteVehicle();
          }
        })
      }
}
