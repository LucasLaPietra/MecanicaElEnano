import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Turno, Vehiculo, state } from 'src/domain/entities';
import { TurnosService } from './turnos.service';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-turnos',
  templateUrl: './turnos.component.html',
  styleUrls: ['./turnos.component.css']
})
export class TurnosComponent implements AfterViewInit {

  selectedDate: Date | undefined;
  selectedTurno: Turno | undefined;
  timeFormControl = new FormControl('', [Validators.required]);
  state: state = 0;
  @Input() selectedVehiculo: Vehiculo | undefined;

  displayedColumns: string[] = ['hora', 'patente', 'vehiculo', 'cliente'];
  dataSource: MatTableDataSource<Turno>;

  @ViewChild(MatTable) turnoTable!: MatTable<Turno>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private turnoService: TurnosService) {
    this.dataSource = new MatTableDataSource();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.turnoService.GetTurnos().subscribe(turnos => {
      this.dataSource.data = turnos.filter((t:Turno)=>{t.fechayHora.getDate() == this.selectedDate?.getDate()});
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  getVehiculos(): void {
    this.turnoService.GetTurnos()
    .subscribe(turnos => turnos.filter((t:Turno)=>{t.fechayHora.getDate() == this.selectedDate?.getDate()}));
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
  }

  updateTurnoButton(){
  }

  cancelUpdateTurno(){
  }

  createTurno(){
  }

  updateTurno(){
    if(this.selectedTurno){
    }
  }

  deleteTurno(){
    if(this.selectedTurno){
    }
  }


  getHora(date: Date):string{
    return date.getTime().toString();
  }

}
