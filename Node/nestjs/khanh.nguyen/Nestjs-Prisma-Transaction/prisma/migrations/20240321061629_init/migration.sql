/*
  Warnings:

  - The primary key for the `Account` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to drop the column `account_id` on the `Account` table. All the data in the column will be lost.
  - You are about to drop the column `account_number` on the `Account` table. All the data in the column will be lost.
  - The primary key for the `Transaction` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to drop the column `account_id` on the `Transaction` table. All the data in the column will be lost.
  - You are about to drop the column `transaction_id` on the `Transaction` table. All the data in the column will be lost.
  - You are about to drop the column `transaction_type` on the `Transaction` table. All the data in the column will be lost.
  - A unique constraint covering the columns `[accountNumber]` on the table `Account` will be added. If there are existing duplicate values, this will fail.
  - Added the required column `accountNumber` to the `Account` table without a default value. This is not possible if the table is not empty.
  - Added the required column `accountId` to the `Transaction` table without a default value. This is not possible if the table is not empty.
  - Added the required column `transactionType` to the `Transaction` table without a default value. This is not possible if the table is not empty.

*/
-- DropForeignKey
ALTER TABLE "Transaction" DROP CONSTRAINT "Transaction_account_id_fkey";

-- DropIndex
DROP INDEX "Account_account_number_key";

-- AlterTable
ALTER TABLE "Account" DROP CONSTRAINT "Account_pkey",
DROP COLUMN "account_id",
DROP COLUMN "account_number",
ADD COLUMN     "accountId" SERIAL NOT NULL,
ADD COLUMN     "accountNumber" TEXT NOT NULL,
ADD CONSTRAINT "Account_pkey" PRIMARY KEY ("accountId");

-- AlterTable
ALTER TABLE "Transaction" DROP CONSTRAINT "Transaction_pkey",
DROP COLUMN "account_id",
DROP COLUMN "transaction_id",
DROP COLUMN "transaction_type",
ADD COLUMN     "accountId" INTEGER NOT NULL,
ADD COLUMN     "transactionId" SERIAL NOT NULL,
ADD COLUMN     "transactionType" TEXT NOT NULL,
ADD CONSTRAINT "Transaction_pkey" PRIMARY KEY ("transactionId");

-- CreateIndex
CREATE UNIQUE INDEX "Account_accountNumber_key" ON "Account"("accountNumber");

-- AddForeignKey
ALTER TABLE "Transaction" ADD CONSTRAINT "Transaction_accountId_fkey" FOREIGN KEY ("accountId") REFERENCES "Account"("accountId") ON DELETE RESTRICT ON UPDATE CASCADE;
