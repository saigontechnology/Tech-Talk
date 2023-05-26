import { ProductModel } from "./product.model";

export interface OrderModel {
    id: string;
    createdTime: string;
    userName: string;
    total: number;
    shipAmount: number | null;
    discount: number | null;
    items: ProductModel[];
}