import { v4 as uuidv4 } from 'uuid';

class Order {
    constructor(product_id, ordered_at, quantity, note) {
        this.id = uuidv4();
        this.product_id = product_id;
        this.ordered_at = ordered_at;
        this.quantity = quantity;
        this.note = note;
    }
}

export default Order;