package com.projectbase.controller;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import java.io.Serializable;

import com.projectbase.tenant.service.ProductService;

import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("/products")
@Slf4j
public class ProductController implements Serializable {

    @Autowired
    ProductService productService;

    @GetMapping
    public ResponseEntity<Object> getAllProduct() {
        log.info("getAllProduct() method call...");
        return new ResponseEntity<>(productService.getAllProduct(),HttpStatus.OK);
    }

}