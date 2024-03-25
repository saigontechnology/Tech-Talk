/*
  Warnings:

  - You are about to drop the `Account` table. If the table is not empty, all the data it contains will be lost.
  - You are about to drop the `Transaction` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropForeignKey
ALTER TABLE "Transaction" DROP CONSTRAINT "Transaction_accountId_fkey";

-- DropTable
DROP TABLE "Account";

-- DropTable
DROP TABLE "Transaction";

-- CreateTable
CREATE TABLE "account" (
    "accountId" SERIAL NOT NULL,
    "accountNumber" TEXT NOT NULL,
    "balance" DOUBLE PRECISION NOT NULL DEFAULT 0,

    CONSTRAINT "account_pkey" PRIMARY KEY ("accountId")
);

-- CreateTable
CREATE TABLE "transaction" (
    "transactionId" SERIAL NOT NULL,
    "accountId" INTEGER NOT NULL,
    "amount" DOUBLE PRECISION NOT NULL,
    "moneySource" "MoneySourceType" NOT NULL,
    "timestamp" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT "transaction_pkey" PRIMARY KEY ("transactionId")
);

-- CreateIndex
CREATE UNIQUE INDEX "account_accountNumber_key" ON "account"("accountNumber");

-- AddForeignKey
ALTER TABLE "transaction" ADD CONSTRAINT "transaction_accountId_fkey" FOREIGN KEY ("accountId") REFERENCES "account"("accountId") ON DELETE RESTRICT ON UPDATE CASCADE;
