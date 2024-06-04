import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdenTrabajo, Vehiculo } from 'src/domain/entities';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { OrdenTrabajosService } from '../orden-trabajo.service';

@Component({
  selector: 'app-orden-trabajo-print',
  templateUrl: './orden-trabajo-print.component.html',
  styleUrls: ['./orden-trabajo-print.component.css']
})
export class OrdenTrabajoPrintComponent implements OnInit {

  ordenTrabajo!: OrdenTrabajo;
  vehiculo!: Vehiculo;

  constructor(    
    private ordenTrabajoService: OrdenTrabajosService,
    private vehiculoService: VehiculosService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.vehiculoService
      .GetVehiculo(this.route.snapshot.paramMap.get('idVehiculo')!)
      .subscribe((vehiculo) => (this.vehiculo = vehiculo));
    this.ordenTrabajoService
      .GetOrdenTrabajo(this.route.snapshot.paramMap.get('idOrdenTrabajo')!)
      .subscribe((ordenTrabajo) => (this.ordenTrabajo = ordenTrabajo));
  }

}
