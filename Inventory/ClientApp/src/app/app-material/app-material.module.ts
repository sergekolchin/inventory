import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
  MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule,
  MatTooltipModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter
} from "@angular/material";
import { MomentUtcDateAdapter } from "../adapters/momentUtcDate.adapter";
import { MAT_MOMENT_DATE_FORMATS } from "@angular/material-moment-adapter";

@NgModule({
  imports: [
    CommonModule, BrowserAnimationsModule,
    MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
    MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule,
    MatTooltipModule
  ],
  declarations: [],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: "en-US" },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter }],
  exports: [
    BrowserAnimationsModule,
    MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
    MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule,
    MatTooltipModule
    ]
})
export class AppMaterialModule { }
