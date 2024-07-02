import { Component, OnInit } from '@angular/core';
import { Presupuesto, Repuesto, Vehiculo } from 'src/domain/entities';
import { PresupuestosService } from '../presupuesto.service';
import { VehiculosService } from 'src/vehiculo/vehiculo.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-presupuesto-print',
  templateUrl: './presupuesto-print.component.html',
  styleUrls: ['./presupuesto-print.component.css']
})
export class PresupuestoPrintComponent implements OnInit {

  presupuesto!: Presupuesto;
  vehiculo!: Vehiculo;
  displayedColumns: string[] = ['Cantidad', 'Descripcion', 'Total'];
  total: number = 0;
  
  constructor(    
    private presupuestoService: PresupuestosService,
    private vehiculoService: VehiculosService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.vehiculoService
      .GetVehiculo(this.route.snapshot.paramMap.get('idVehiculo')!)
      .subscribe((vehiculo) => (this.vehiculo = vehiculo));
    this.presupuestoService
      .GetPresupuesto(this.route.snapshot.paramMap.get('idPresupuesto')!)
      .subscribe((presupuesto) => {(this.presupuesto = presupuesto); this.getTotal()});
  }

  getTotal(){
    this.presupuesto.repuestos.forEach((r: Repuesto) => {this.total += this.getTotalFila(r)})
  }

  getTotalFila(r: Repuesto): number{
    return r.cantidad*r.precio
  }
}
