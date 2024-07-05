package com.projectbase.mastertenant.entity;

import javax.persistence.*;
import java.io.Serializable;

@Entity
@Table(name = "tbl_tenant_master")
public class MasterTenant implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "tenant_client_id")
    private Integer tenantClientId;

    @Column(name = "db_name",nullable = false)
    private String dbName;

    @Column(name = "url",nullable = false)
    private String url;

    @Column(name = "user_name",nullable = false)
    private String userName;

    @Column(name = "password",nullable = false)
    private String password;

    @Column(name = "driver_class",nullable = false)
    private String driverClass;

    @Column(name = "status",nullable = false)
    private String status;

    public MasterTenant() {
    }

    public MasterTenant(String dbName, String url, String userName, String password, String driverClass, String status) {
        this.dbName = dbName;
        this.url = url;
        this.userName = userName;
        this.password = password;
        this.driverClass = driverClass;
        this.status = status;
    }

    public Integer getTenantClientId() {
        return tenantClientId;
    }

    public MasterTenant setTenantClientId(Integer tenantClientId) {
        this.tenantClientId = tenantClientId;
        return this;
    }

    public String getDbName() {
        return dbName;
    }

    public MasterTenant setDbName(String dbName) {
        this.dbName = dbName;
        return this;
    }

    public String getUrl() {
        return url;
    }

    public MasterTenant setUrl(String url) {
        this.url = url;
        return this;
    }

    public String getUserName() {
        return userName;
    }

    public MasterTenant setUserName(String userName) {
        this.userName = userName;
        return this;
    }

    public String getPassword() {
        return password;
    }

    public MasterTenant setPassword(String password) {
        this.password = password;
        return this;
    }

    public String getDriverClass() {
        return driverClass;
    }

    public MasterTenant setDriverClass(String driverClass) {
        this.driverClass = driverClass;
        return this;
    }

    public String getStatus() {
        return status;
    }

    public MasterTenant setStatus(String status) {
        this.status = status;
        return this;
    }
}