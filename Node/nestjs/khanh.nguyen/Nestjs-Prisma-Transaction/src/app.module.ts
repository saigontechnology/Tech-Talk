import { Module } from "@nestjs/common";
import { AccountsModule } from "./accounts/accounts.module";
import { PrismaModule } from "./prisma/prisma.module";
import { TransactionsModule } from "./transactions/transactions.module";

@Module({
  imports: [AccountsModule, PrismaModule, TransactionsModule],
  controllers: [],
  providers: [],
})
export class AppModule {}
