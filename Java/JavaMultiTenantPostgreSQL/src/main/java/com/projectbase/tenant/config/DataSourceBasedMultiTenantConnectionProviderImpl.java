package com.projectbase.tenant.config;

import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import javax.sql.DataSource;

import org.hibernate.engine.jdbc.connections.spi.AbstractDataSourceBasedMultiTenantConnectionProviderImpl;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Configuration;

import com.projectbase.mastertenant.config.DBContextHolder;
import com.projectbase.mastertenant.entity.MasterTenant;
import com.projectbase.mastertenant.repository.MasterTenantRepository;
import com.projectbase.util.DataSourceUtil;

import lombok.extern.slf4j.Slf4j;

@Configuration
@Slf4j
public class DataSourceBasedMultiTenantConnectionProviderImpl
        extends AbstractDataSourceBasedMultiTenantConnectionProviderImpl {

    private static final long serialVersionUID = 1L;

    private Map<String, DataSource> dataSourcesMtApp = new TreeMap<>();

    @Autowired
    private MasterTenantRepository masterTenantRepository;

    @Autowired
    ApplicationContext applicationContext;

    @Override
    protected DataSource selectAnyDataSource() {
        // This method is called more than once. So check if the data source map
        // is empty. If it is then rescan master_tenant table for all tenant
        if (dataSourcesMtApp.isEmpty()) {
            List<MasterTenant> masterTenants = masterTenantRepository.findAll();
            log.info("selectAnyDataSource() method call...Total tenants:" + masterTenants.size());
            for (MasterTenant masterTenant : masterTenants) {
                DataSource dataSource = DataSourceUtil.createAndConfigureDataSource(masterTenant);
                dataSourcesMtApp.put(masterTenant.getDbName(), dataSource);
            }
        }
        return this.dataSourcesMtApp.values().iterator().next();
    }

    @Override
    protected DataSource selectDataSource(String tenantIdentifier) {
        // If the requested tenant id is not present check for it in the master
        // database 'master_tenant' table
        tenantIdentifier = initializeTenantIfLost(tenantIdentifier);
        if (!this.dataSourcesMtApp.containsKey(tenantIdentifier)) {
            List<MasterTenant> masterTenants = masterTenantRepository.findAll();
            log.info("selectDataSource() method call...Tenant:" + tenantIdentifier + " Total tenants:" + masterTenants.size());
            for (MasterTenant masterTenant : masterTenants) {
                dataSourcesMtApp.put(masterTenant.getDbName(), DataSourceUtil.createAndConfigureDataSource(masterTenant));
            }
        }
        //check again if tenant exist in map after rescan master_db, if not, throw UsernameNotFoundException
        if (!this.dataSourcesMtApp.containsKey(tenantIdentifier)) {
            log.warn("Trying to get tenant:" + tenantIdentifier + " which was not found in master db after rescan");
            throw new IllegalArgumentException(String.format("Tenant not found after rescan, " + " tenant=%s", tenantIdentifier));
        }
        return this.dataSourcesMtApp.get(tenantIdentifier);
    }

    private String initializeTenantIfLost(String tenantIdentifier) {
        if (!tenantIdentifier.equals(DBContextHolder.getCurrentTenant())) {
            tenantIdentifier = DBContextHolder.getCurrentTenant();
        }
        return tenantIdentifier;
    }
}
