/*
  Warnings:

  - You are about to drop the column `transactionType` on the `Transaction` table. All the data in the column will be lost.
  - Added the required column `moneySource` to the `Transaction` table without a default value. This is not possible if the table is not empty.

*/
-- CreateEnum
CREATE TYPE "MoneySourceType" AS ENUM ('MOMO', 'VPBANK');

-- AlterTable
ALTER TABLE "Transaction" DROP COLUMN "transactionType",
ADD COLUMN     "moneySource" "MoneySourceType" NOT NULL;
