import { Component, OnInit } from '@angular/core';
import { Repuesto, Trabajo, Vehiculo } from 'src/domain/entities';
import { TrabajosService } from '../trabajo.service';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-trabajo-print',
  templateUrl: './trabajo-print.component.html',
  styleUrls: ['./trabajo-print.component.css']
})
export class TrabajoPrintComponent implements OnInit {

  trabajo!: Trabajo;
  vehiculo!: Vehiculo;
  displayedColumns: string[] = ['Cantidad', 'Descripcion', 'Total'];
  total: number = 0;

  constructor(    
    private trabajoService: TrabajosService,
    private vehiculoService: VehiculosService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.vehiculoService
      .GetVehiculo(this.route.snapshot.paramMap.get('idVehiculo')!)
      .subscribe((vehiculo) => (this.vehiculo = vehiculo));
    this.trabajoService
      .GetTrabajo(this.route.snapshot.paramMap.get('idTrabajo')!)
      .subscribe((trabajo) => {
        (this.trabajo = trabajo); 
        this.getTotal();
        console.log(trabajo.trabajosRealizados);
      });
  }

  getTotal(){
    this.trabajo.repuestos.forEach((r: Repuesto) => {this.total += r.cantidad*r.precio})
  }
  
  formatWithBullets(data:string): string {
    return data.split('\n').map(item => `• ${item}`).join('<br>');
  }
}
