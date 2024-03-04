package com.eventsourcing.bankAccount.events;

import java.math.BigDecimal;

import com.eventsourcing.bankAccount.domain.BankAccountAggregate;
import com.eventsourcing.es.BaseEvent;

import lombok.Builder;
import lombok.Data;

@Data
public class BalanceWithdrawedEvent extends BaseEvent {
    public static final String BALANCE_WITHDRAWED = "BALANCE_WITHDRAWED_V1";
    public static final String AGGREGATE_TYPE = BankAccountAggregate.AGGREGATE_TYPE;

    private BigDecimal amount;

    @Builder
    public BalanceWithdrawedEvent(String aggregateId, BigDecimal amount) {
        super(aggregateId);
        this.amount = amount;
    }
}