Create master_db database

``create database master_db``

Create tbl_tenant_master table
```
CREATE TABLE tbl_tenant_master (
    tenant_client_id int NOT NULL,
    db_name varchar(50) NOT null,
    url varchar(100) NOT NULL,
    user_name varchar(50) NOT NULL,
    password varchar(100) NOT NULL,
    driver_class varchar(100) NOT NULL,
    status varchar(10) NOT NULL,
    PRIMARY KEY (tenant_client_id)
)
```

Insert master data tbl_tenant_master
```
insert into tbl_tenant_master values (1, 'tenant_1_db', 'jdbc:postgresql://localhost:5432/tenant_1_db?useSSL=false', 'root', 'root', 'org.postgresql.Driver', 'Active');
insert into tbl_tenant_master values (2, 'tenant_2_db', 'jdbc:postgresql://localhost:5432/tenant_2_db?useSSL=false', 'root', 'root', 'org.postgresql.Driver', 'Active');
select * from tbl_tenant_master;
```

Create tenant_1_db database
```
CREATE TABLE  tbl_product (
product_id SERIAL PRIMARY KEY,
product_name varchar(50) NOT NULL,
quantity int NOT NULL DEFAULT 0,
size varchar(3) NOT NULL
)
```
Insert data for tenant_1_db

```
insert into tbl_product(product_name, quantity, size) values ('Mi Goi Hao Hao', 10, '3x6');
select * from tbl_product
```

Create tenant_2_db database
```
CREATE TABLE  tbl_product (
product_id SERIAL PRIMARY KEY,
product_name varchar(50) NOT NULL,
quantity int NOT NULL DEFAULT 0,
size varchar(3) NOT NULL
)
```
Insert data for tenant_2_db
```
insert into tbl_product(product_name, quantity, size) values ('Mi Goi Lau Thai', 20, '3x6');
select * from tbl_product
```

