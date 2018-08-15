import { Warehouse } from "./warehouse";

export class Product {
  constructor(
    public id: number,
    public name: string,
    public type: string,
    public expiryDate: Date,
    public warehouseId: number,
    public warehouse: Warehouse
  ) { }
}
