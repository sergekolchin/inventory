import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AppComponent } from "./app.component";
import { HomeComponent, AddProductDialog as AddEmployeeDialog } from "./home/home.component";
import { MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
  MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter
} from "@angular/material";
import { MomentUtcDateAdapter } from "./adapters/momentUtcDate.adapter";
import { MAT_MOMENT_DATE_FORMATS } from "@angular/material-moment-adapter";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AddEmployeeDialog
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
    MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent, pathMatch: "full" },
      { path: "**", redirectTo: "/" }
    ])
  ],
  entryComponents: [
    HomeComponent,
    AddEmployeeDialog
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: "en-US" },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter }],
  bootstrap: [AppComponent]
})
export class AppModule { }
