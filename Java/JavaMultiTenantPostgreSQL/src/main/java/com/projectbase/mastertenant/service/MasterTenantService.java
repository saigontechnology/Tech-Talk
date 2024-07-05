package com.projectbase.mastertenant.service;

import com.projectbase.mastertenant.entity.MasterTenant;

public interface MasterTenantService {

    MasterTenant findByClientId(Integer clientId);
}