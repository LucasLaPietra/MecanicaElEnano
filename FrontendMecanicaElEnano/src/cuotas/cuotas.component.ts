import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Cuota, CuotaResponse } from 'src/domain/cuotas';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-cuotas',
  templateUrl: './cuotas.component.html',
  styleUrls: ['./cuotas.component.css']
})
export class CuotasComponent implements OnInit {

  @Input() costo = 0;

  displayedColumns: string[] = ['numero', 'precio'];

  values: Cuota[] = [
   ];

   multipliers: number[]= [];

   dataSource!: Cuota[];

  updateDataSource() {
    for (let i = 0; i < this.values.length; i++) {
      this.values[i].precio = (Number)((this.costo * this.multipliers[i]).toPrecision(2));
    }
  }

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.http.get("assets/cuotas.txt", { responseType: 'text'}).subscribe((data) => {
      let dataArr: CuotaResponse[]= JSON.parse(data);
      this.multipliers = dataArr.map(d => d.multiplicador);
      dataArr.forEach(cuotaResponse => {
        this.values.push({numero:cuotaResponse.numero, precio:0});
      });
      this.updateDataSource();
      this.dataSource = [...this.values];
      console.log(this.values);
  })
  }

  ngOnChanges(changes: SimpleChanges) {
    this.updateDataSource();
  }

}
