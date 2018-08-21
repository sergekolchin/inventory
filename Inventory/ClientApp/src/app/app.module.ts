/// <reference path="app-material/app-material.module.ts" />
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { HomeComponent, AddProductDialog as AddEmployeeDialog } from "./home/home.component";
import { AppMaterialModule } from "./app-material/app-material.module";

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
    AppMaterialModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent, pathMatch: "full" },
      { path: "**", redirectTo: "/" }
    ])
  ],
  entryComponents: [
    HomeComponent,
    AddEmployeeDialog
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
