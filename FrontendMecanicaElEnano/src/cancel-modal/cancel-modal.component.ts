import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DialogData } from 'src/domain/dialogData';

@Component({
  selector: 'app-cancel-modal',
  templateUrl: './cancel-modal.component.html',
  styleUrls: ['./cancel-modal.component.css']
})
export class CancelModalComponent implements OnInit {

  public dialog!: MatDialogRef<CancelModalComponent>;

  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData) {}

  cerrarDialogo(): void {
    this.dialog.close(false);
  }
  confirmado(): void {
    this.dialog.close(true);
  }

  ngOnInit(): void {
  }

}
