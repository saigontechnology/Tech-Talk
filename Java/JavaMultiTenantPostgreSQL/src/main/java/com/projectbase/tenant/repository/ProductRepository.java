package com.projectbase.tenant.repository;


import org.springframework.data.jpa.repository.JpaRepository;

import com.projectbase.tenant.entity.Product;

public interface ProductRepository extends JpaRepository<Product, Integer>{
}