import { Injectable } from "@nestjs/common";
import { CreateTransactionDto } from "./dto/create-transaction.dto";
import { PrismaService } from "src/prisma/prisma.service";

@Injectable()
export class TransactionsService {
  constructor(private prisma: PrismaService) {}
  async create(createTransactionDto: CreateTransactionDto) {
    await this.prisma.$transaction([
      this.prisma.account.update({
        data: {
          balance: {
            increment: createTransactionDto.amount,
          },
        },
        where: {
          accountId: createTransactionDto.accountId,
        },
      }),
      this.prisma.transaction.create({
        data: createTransactionDto as any,
      }),
    ]);
    return "This action adds a new transaction";
  }

  findAll() {
    return `This action returns all transactions`;
  }

  findOne(id: number) {
    return `This action returns a #${id} transaction`;
  }
}
