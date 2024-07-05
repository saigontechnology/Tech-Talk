package com.projectbase;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ComponentScan;

@SpringBootApplication
@ComponentScan("com.projectbase")
public class JavaMultiTenantPostgreSQLApplication{

	public static void main(String[] args) {
		SpringApplication.run(JavaMultiTenantPostgreSQLApplication.class, args);
	}

}
