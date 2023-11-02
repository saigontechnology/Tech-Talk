import { Controller, Get, UseGuards } from '@nestjs/common';
import { AuthGuard } from '@nestjs/passport';

@Controller('users')
@UseGuards(AuthGuard('auth0'))
export class UsersController {
  @Get()
  getInfo() {
    return {
      name: 'Tan Vo',
      email: 'tan.vo@saigontechnology.com',
    };
  }
}
