import { Component, OnInit, ViewChild, Inject } from "@angular/core";
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { DataService } from "../services/dataService";
import { Product } from "../models/product";
import { Warehouse } from "../models/warehouse";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html"
})
export class HomeComponent implements OnInit {
  displayedColumns: string[] = ["id", "name", "type", "expiryDate", "warehouse"];

  dataSource: MatTableDataSource<Product>;
  products: Product[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild((MatSort) as any) sort: MatSort;

  constructor(public dialog: MatDialog, private readonly dataService: DataService) { }

  ngOnInit(): void {
    this.dataService.getProducts().subscribe(result => {
      this.products = result;
      this.dataSource = new MatTableDataSource(this.products);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }, error => {
      console.log("getProducts() error: ", error);
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddProductDialog, {
      width: "450px",
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataService.addProduct(result).subscribe(product => {
          this.products.push(product);
          //refresh table
          this.dataSource.data = this.products;
        },
          error => console.log("addProduct() error", error));
      }
    });
  }
}

@Component({
  selector: "add-employee-dialog",
  templateUrl: "./addProduct.dialog.html",
})
export class AddProductDialog implements OnInit {
  formGroup: FormGroup;
  warehouses: Warehouse[];

  constructor(public dialogRef: MatDialogRef<AddProductDialog>, @Inject(MAT_DIALOG_DATA) public data: Product, fb: FormBuilder,
    private readonly dataService: DataService) {
    this.formGroup = fb.group({
      id: fb.control(!data.id ? 0 : data.id),
      name: fb.control("", [Validators.required]),
      type: fb.control(""),
      expiryDate: fb.control(""),
      warehouseId: fb.control("", [Validators.required])
    });
  }

  ngOnInit(): void {
    this.dataService.getWarhouses().subscribe(result => {
      this.warehouses = result;
    }, error => {
      console.log("getWarhouses() error: ", error);
    });
  }

  submit(form) {
    this.dialogRef.close(form.value);
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}
