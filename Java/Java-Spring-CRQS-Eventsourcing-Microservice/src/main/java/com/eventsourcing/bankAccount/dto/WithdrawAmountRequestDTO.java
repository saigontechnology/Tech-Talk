package com.eventsourcing.bankAccount.dto;

import java.math.BigDecimal;

import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;

public record WithdrawAmountRequestDTO(@Min(value = 300, message = "minimal amount is 300") @NotNull BigDecimal amount) {
}