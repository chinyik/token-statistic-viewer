import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Token } from '../../models/token';

@Component({
  selector: 'app-token-view-dialog',
  templateUrl: './token-view-dialog.component.html',
  styleUrls: ['./token-view-dialog.component.css']
})
export class TokenViewDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<TokenViewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Token) {
    console.log(data);
    }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
