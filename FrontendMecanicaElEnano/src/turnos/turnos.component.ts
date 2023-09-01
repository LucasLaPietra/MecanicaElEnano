import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Turno, Vehiculo, state } from 'src/domain/entities';
import { TurnosService } from './turnos.service';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { CreateTurno } from 'src/domain/dto';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-turnos',
  templateUrl: './turnos.component.html',
  styleUrls: ['./turnos.component.css']
})
export class TurnosComponent implements AfterViewInit {

  turnos: Turno[] = [];
  selectedDate: Date = new Date();
  selectedTurno: Turno | null = null;
  dateFormControl = new FormControl(new Date(), [Validators.required])
  timeFormControl = new FormControl("", [Validators.required]);
  detalleFormControl = new FormControl("", [Validators.required]);
  state: state = 0;
  selectedVehiculo: Vehiculo | undefined;

  displayedColumns: string[] = ['hora', 'patente', 'vehiculo', 'cliente'];
  dataSource: MatTableDataSource<Turno>;

  @ViewChild(MatTable) turnoTable!: MatTable<Turno>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private turnoService: TurnosService,
    private vehiculoService: VehiculosService,
    private route: ActivatedRoute)
    {
    this.dataSource = new MatTableDataSource();
  }

  ngAfterViewInit() {
    if (this.route.snapshot.paramMap.get('date')) {
      this.state=state.creating;
      this.selectedDate= new Date(this.route.snapshot.paramMap.get('date')!);
      this.vehiculoService.GetVehiculo((this.route.snapshot.paramMap.get('id')!))
      .subscribe(vehiculo => this.selectedVehiculo = vehiculo);
    }
    else{
      this.state=state.viewing;
      this.detalleFormControl.disable();
      this.timeFormControl.disable()
    }
    this.timeFormControl.setValue(this.getTimeAsString(this.selectedDate));
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.turnoService.GetTurnos().subscribe(turnos => {
      console.log(new Date(turnos[1].fechayHora))
      console.log(this.selectedDate)
      this.turnos=turnos
      this.dataSource.data = this.turnos.filter((t:Turno)=>this.formatDate(new Date(t.fechayHora)) == this.formatDate(this.selectedDate));
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  selectDate(){
    console.log(new Date(this.turnos[1].fechayHora))
    console.log(this.selectedDate)
    console.log(this.turnos)
    this.dataSource.data = this.turnos.filter((t:Turno)=>this.formatDate(new Date(t.fechayHora)) == this.formatDate(this.selectedDate));
  }

  getTimeAsString(date: Date) {
    return `${date.getHours()==0?"00":date.getHours()}:${(date.getMinutes() < 10 ? "0" : "") +
      date.getMinutes()}`;
  }

  formatDate(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear().toString();

    return `${day}:${month}:${year}`;
  }

  getTurnos(): void {
    this.turnoService.GetTurnos()
    .subscribe(turnos => turnos.filter((t:Turno)=>this.formatDate(t.fechayHora) == this.formatDate(this.selectedDate)));
    this.turnoTable.renderRows();
    this.dataSource._updateChangeSubscription();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  selectTurno(row: Turno){
    this.cancelCreateTurno();
    this.cancelUpdateTurno();
    if (this.selectedTurno!=row)
    {
      this.selectedTurno=row;
      console.log(new Date(row.fechayHora).getHours());
      this.timeFormControl.patchValue(this.getTimeAsString(new Date(row.fechayHora)));
      this.detalleFormControl.patchValue(row.detalle)
    }
    else
    {
      this.selectedTurno = null;
      this.timeFormControl.patchValue("08:00");
      this.detalleFormControl.reset();
    }
  }

  updateTurnoButton(){
    this.state=state.updating;
    this.detalleFormControl.enable();
    this.timeFormControl.enable();
  }

  cancelUpdateTurno(){
    this.state=state.viewing;
    this.timeFormControl.patchValue(this.getTimeAsString(new Date(this.selectedTurno?.fechayHora!)));
    this.detalleFormControl.patchValue(this.selectedTurno?.detalle!);
    this.detalleFormControl.disable();
    this.timeFormControl.disable()
  }

  createTurno(){
    let completeDate: Date = this.selectedDate;
    const parts = this.timeFormControl.value!.split(":");
    completeDate.setUTCHours(parseInt(parts[0], 10));
    completeDate.setUTCMinutes(parseInt(parts[1], 10));
    completeDate.setUTCSeconds(0)
    console.log(completeDate);
    const turnoACrear: CreateTurno = {
      detalle : this.detalleFormControl.value!,
      fechayHora: completeDate,
      vehiculoId: this.route.snapshot.paramMap.get('id')!,
    }
    const turnoNuevo: Turno = {
      turnoId: "",
      detalle : this.detalleFormControl.value!,
      fechayHora: completeDate,
      vehiculo: this.selectedVehiculo!
    }
    this.turnoService.CreateTurno(turnoACrear)
    .subscribe(turno => {
      completeDate.setHours(parseInt(parts[0], 10));
      completeDate.setMinutes(parseInt(parts[1], 10));
      completeDate.setSeconds(0)
      turnoNuevo.fechayHora=completeDate
      this.dataSource.data.push(turnoNuevo);
      this.turnoTable.renderRows();
      this.dataSource._updateChangeSubscription();
    });
    this.state=state.viewing;
    this.detalleFormControl.disable();
    this.timeFormControl.disable();
  }

  cancelCreateTurno(){
    this.state=state.viewing;
    this.selectedVehiculo=undefined;
    this.detalleFormControl.disable();
    this.timeFormControl.disable();
  }

  updateTurno(){
    if(this.selectedTurno){
      const indexOfObject =  this.dataSource.data.indexOf(this.selectedTurno);
      let turnoActualizado: Turno = this.selectedTurno;
      turnoActualizado.detalle = this.detalleFormControl.value?this.detalleFormControl.value:""
      let completeDate: Date = new Date(this.selectedTurno.fechayHora);
      const parts = this.timeFormControl.value!.split(":");
      completeDate.setUTCHours(parseInt(parts[0], 10));
      completeDate.setUTCMinutes(parseInt(parts[1], 10));
      completeDate.setUTCSeconds(0)
      turnoActualizado.fechayHora = completeDate;
      this.turnoService.UpdateTurno(turnoActualizado).subscribe(turno => {
        completeDate.setHours(parseInt(parts[0], 10));
        completeDate.setMinutes(parseInt(parts[1], 10));
        completeDate.setSeconds(0)
        turno.fechayHora=completeDate
        this.dataSource.data[indexOfObject] = turno;
      });
      this.turnoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.state=state.viewing;
      this.detalleFormControl.disable();
      this.timeFormControl.disable();
    }
  }

  deleteTurno(){
    if(this.selectedTurno){
      this.turnoService.DeleteTurno(this.selectedTurno.turnoId).subscribe();
      this.dataSource.data = this.dataSource.data.filter(h => h !== this.selectedTurno);
      this.turnoTable.renderRows();
      this.dataSource._updateChangeSubscription();
      this.selectedTurno = null;
      this.timeFormControl.patchValue("08:00");
      this.detalleFormControl.reset();
    }
  }

}
