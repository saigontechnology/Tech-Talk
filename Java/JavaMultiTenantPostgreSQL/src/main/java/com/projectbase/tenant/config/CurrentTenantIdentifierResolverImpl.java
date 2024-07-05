package com.projectbase.tenant.config;

import org.apache.commons.lang3.StringUtils;
import org.hibernate.context.spi.CurrentTenantIdentifierResolver;
import org.springframework.stereotype.Component;

import com.projectbase.mastertenant.config.DBContextHolder;

import lombok.extern.slf4j.Slf4j;

@Slf4j
@Component
public class CurrentTenantIdentifierResolverImpl
        implements CurrentTenantIdentifierResolver{

    private static final String DEFAULT_TENANT_ID = "client_tenant_1";

    @Override
    public String resolveCurrentTenantIdentifier(){
        String tenant = DBContextHolder.getCurrentTenant();
        log.info("Resolving current tenant identifier for tenant-id {}", tenant);
        return StringUtils.isNotBlank(tenant) ? tenant : DEFAULT_TENANT_ID;
    }

    @Override
    public boolean validateExistingCurrentSessions(){
        return true;
    }
}
