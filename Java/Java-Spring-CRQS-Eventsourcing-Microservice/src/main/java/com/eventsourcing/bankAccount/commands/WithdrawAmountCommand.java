package com.eventsourcing.bankAccount.commands;

import java.math.BigDecimal;

public record WithdrawAmountCommand(String aggregateID, BigDecimal amount){

}
