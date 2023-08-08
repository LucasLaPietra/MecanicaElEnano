import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Cuota } from 'src/domain/cuotas';

@Component({
  selector: 'app-cuotas',
  templateUrl: './cuotas.component.html',
  styleUrls: ['./cuotas.component.css']
})
export class CuotasComponent implements OnInit {

  @Input() costo = 0;

  displayedColumns: string[] = ['numero', 'precio'];

  values: Cuota[] = [
    {numero: 3, precio: 0},
    {numero: 6, precio: 0},
    {numero: 9, precio: 0},
    {numero: 12, precio: 0},
    {numero: 18, precio: 0},
   ];

   multipliers = [1.3, 1.6, 1.8, 2, 2.2];

   dataSource!: Cuota[];

  updateDataSource() {
    for (let i = 0; i < this.values.length; i++) {
      this.values[i].precio = (Number)((this.costo * this.multipliers[i]).toPrecision(2));
    }
  }

  constructor() { }

  ngOnInit(): void {
    this.updateDataSource();
    this.dataSource = [...this.values];
    console.log(this.values);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.updateDataSource();
  }

}
