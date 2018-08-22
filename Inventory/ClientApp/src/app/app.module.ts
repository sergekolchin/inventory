import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { HomeComponent, AddProductDialog as AddEmployeeDialog } from "./home/home.component";
import { AppMaterialModule } from "./app-material/app-material.module";
import { PlatformLocation, APP_BASE_HREF } from "@angular/common";

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
  providers: [
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [PlatformLocation]
    }
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }

// get a string instance of the '<base href="" />' value from 'index.html'.
export function getBaseHref(platformLocation: PlatformLocation): string {
  return platformLocation.getBaseHrefFromDOM();
}
