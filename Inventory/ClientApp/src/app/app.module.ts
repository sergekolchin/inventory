import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AppComponent } from "./app.component";
import { HomeComponent, AddProductDialog as AddEmployeeDialog } from "./home/home.component";
import { MatFormFieldModule, MatInputModule, MatRadioModule, MatSelectModule, MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatIconModule,
  MatTableModule, MatPaginatorModule, MatProgressSpinnerModule, MatSortModule, MatGridListModule, MatDialogModule, MatNativeDateModule, MatListModule
} from "@angular/material";

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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
