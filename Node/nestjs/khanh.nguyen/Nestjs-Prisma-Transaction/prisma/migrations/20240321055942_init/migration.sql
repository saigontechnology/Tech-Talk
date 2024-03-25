-- CreateTable
CREATE TABLE "Account" (
    "account_id" SERIAL NOT NULL,
    "account_number" TEXT NOT NULL,
    "balance" DOUBLE PRECISION NOT NULL DEFAULT 0,

    CONSTRAINT "Account_pkey" PRIMARY KEY ("account_id")
);

-- CreateTable
CREATE TABLE "Transaction" (
    "transaction_id" SERIAL NOT NULL,
    "account_id" INTEGER NOT NULL,
    "amount" DOUBLE PRECISION NOT NULL,
    "transaction_type" TEXT NOT NULL,
    "timestamp" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT "Transaction_pkey" PRIMARY KEY ("transaction_id")
);

-- CreateIndex
CREATE UNIQUE INDEX "Account_account_number_key" ON "Account"("account_number");

-- AddForeignKey
ALTER TABLE "Transaction" ADD CONSTRAINT "Transaction_account_id_fkey" FOREIGN KEY ("account_id") REFERENCES "Account"("account_id") ON DELETE RESTRICT ON UPDATE CASCADE;
