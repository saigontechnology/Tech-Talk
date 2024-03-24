import { Injectable } from "@nestjs/common";
import { CreateAccountDto } from "./dto/create-account.dto";
import { PrismaService } from "src/prisma/prisma.service";

@Injectable()
export class AccountsService {
  constructor(private prisma: PrismaService) {}
  async create(createAccountDto: CreateAccountDto) {
    await this.prisma.account.create({ data: createAccountDto });
    return;
  }

  async findAll() {
    return await this.prisma.account.findMany();
  }

  async findOne(id: number) {
    return await this.prisma.account.findUnique({ where: { accountId: id } });
  }
}
