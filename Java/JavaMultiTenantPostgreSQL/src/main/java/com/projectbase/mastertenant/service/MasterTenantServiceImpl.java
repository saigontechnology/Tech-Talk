package com.projectbase.mastertenant.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.projectbase.mastertenant.entity.MasterTenant;
import com.projectbase.mastertenant.repository.MasterTenantRepository;

import lombok.extern.slf4j.Slf4j;

@Service
@Slf4j
public class MasterTenantServiceImpl implements MasterTenantService{

    @Autowired
    MasterTenantRepository masterTenantRepository;

    @Override
    public MasterTenant findByClientId(Integer clientId) {
        log.info("findByClientId() method call...");
        return masterTenantRepository.findByTenantClientId(clientId);
    }
}