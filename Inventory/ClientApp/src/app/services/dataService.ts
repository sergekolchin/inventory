import { Injectable } from "@angular/core";
import { Warehouse } from "../models/warehouse";
import { Product } from "../models/product";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

@Injectable({ providedIn: "root" })
export class DataService {
  readonly productsUrl = "api/products";
  readonly warehousestUrl = "api/warehouses";

  constructor(private readonly http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productsUrl);
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(this.productsUrl + "/" + id);
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.productsUrl, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(this.productsUrl + "/" + product.id, product);
  }

  deleteProduct(id: number): Observable<Product> {
    return this.http.delete<Product>(this.productsUrl + "/" + id);
  }

  getWarhouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.warehousestUrl);
  }
}
