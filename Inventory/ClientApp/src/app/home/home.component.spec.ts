import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { HomeComponent } from "./home.component";
import { HttpClientModule } from "@angular/common/http";
import { AppMaterialModule } from "../app-material/app-material.module";

describe("HomeComponent", () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [HomeComponent],
      imports: [AppMaterialModule, HttpClientModule]
      })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
