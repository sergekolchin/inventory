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
  displayedColumns: string[] = ["id", "name", "type", "expiryDate", "warehouse", "actionsColumn"];

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

  openDialog(row: Product): void {
    const dialogRef = this.dialog.open(AddProductDialog, {
      width: "450px",
      data: !row ? {} : row 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result as Product) {
        if (result.id === 0) {
          // add new product
          this.dataService.addProduct(result).subscribe(product => {
            this.products.push(product);
            this.refreshTable();
          },
            error => {
              console.log("addProduct() error", error);
            });
        } else {
          // update existing product
          this.dataService.updateProduct(result).subscribe(product => {
            let index = this.products.findIndex(x => x.id === result.id);
            this.products[index] = product;
            this.refreshTable();
          },
            error => {
              console.log("updateProduct() error", error);
            });
        }
      }
    });
  }

  delete(id): void {
    this.dataService.deleteProduct(id).subscribe(product => {
      let index = this.products.findIndex(x => x.id === id);
      this.products.splice(index, 1);
      this.refreshTable();
    }, error => {
      console.log("deleteProduct()", error);
    });
  }

  edit(row: Product): void {
    this.openDialog(row);
  }

  refreshTable(): void {
    this.dataSource.data = this.products;
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
      name: fb.control(data.name, [Validators.required]),
      type: fb.control(data.type),
      expiryDate: fb.control(data.expiryDate),
      warehouseId: fb.control(!data.warehouseId ? 1 : data.warehouseId, [Validators.required]) //set default value for select
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
