import { IsNotEmpty, IsNumber, IsString } from "class-validator";
export class CreateTransactionDto {
  @IsNumber()
  @IsNotEmpty()
  accountId: number;

  @IsNumber()
  @IsNotEmpty()
  amount: number;

  @IsString()
  @IsNotEmpty()
  moneySource: string;
}
