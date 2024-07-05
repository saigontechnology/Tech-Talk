package com.projectbase.tenant.entity;

import javax.persistence.*;
import java.io.Serializable;

@Entity
@Table(name = "tbl_product")
public class Product implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private Integer productId;

    @Column(name = "product_name",nullable = false)
    private String productName;

    @Column(name = "quantity",nullable = false)
    private String quantity;

    @Column(name = "size",nullable = false,unique = true)
    private String size;

    public Product() {
    }

    public Product(String productName, String quantity, String size) {
        this.productName = productName;
        this.quantity = quantity;
        this.size = size;
    }

    public Integer getProductId() {
        return productId;
    }

    public Product setProductId(Integer productId) {
        this.productId = productId;
        return this;
    }

    public String getProductName() {
        return productName;
    }

    public Product setProductName(String productName) {
        this.productName = productName;
        return this;
    }

    public String getQuantity() {
        return quantity;
    }

    public Product setQuantity(String quantity) {
        this.quantity = quantity;
        return this;
    }

    public String getSize() {
        return size;
    }

    public Product setSize(String size) {
        this.size = size;
        return this;
    }
}