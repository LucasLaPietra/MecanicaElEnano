import { Component, Input, OnInit, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-cuotas',
  templateUrl: './cuotas.component.html',
  styleUrls: ['./cuotas.component.css']
})
export class CuotasComponent implements OnInit {

  @Input() precio = 0;

  displayedColumns: string[] = ['numero', 'precio'];

  values= new Map([
    ['3', 0],
    ['6', 0],
    ['9', 0],
    ['12', 0],
    ['18', 0]
   ]);

   multipliers= [1.3,1.6,1.8,2,2.2]

   dataSource=[...this.values]
   
  updateDataSource(){
    this.values.set('3',this.precio*this.multipliers[1]);
    this.values.set('6',this.precio*this.multipliers[2]);
    this.values.set('9',this.precio*this.multipliers[3]);
    this.values.set('12',this.precio*this.multipliers[4]);
    this.values.set('18',this.precio*this.multipliers[5]);
  }
  constructor() { }

  ngOnInit(): void {
    this.updateDataSource();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.updateDataSource();
  }

}
