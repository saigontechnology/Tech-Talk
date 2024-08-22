import {
  Body,
  Controller,
  Get,
  Param,
  ParseIntPipe,
  Post,
  UseFilters,
  UseGuards,
  UseInterceptors,
  UsePipes,
} from '@nestjs/common';
import { HttpExceptionFilter } from 'src/core/filters/http-exception.filter';
import { AuthGuard } from 'src/core/guards/auth.guard';
import { LoggingInterceptor } from 'src/core/interceptors/logging.interceptor';
import { ValidationPipe } from 'src/core/pipes/validation.pipe';
import { UsersDto } from './dto/users.dto';
import { UsersService } from './users.service';

@Controller('users')
export class UsersController {
  constructor(private readonly userService: UsersService) {}

  @Get()
  getUsers() {
    return this.userService.getUsers();
  }

  @UseFilters(HttpExceptionFilter)
  @UseGuards(AuthGuard)
  @UseInterceptors(LoggingInterceptor)
  @UsePipes(ValidationPipe)
  @Get(':id')
  getUserById(@Param('id', ParseIntPipe) id: number) {
    return this.userService.getUserById(id);
  }

  @Post()
  createUser(@Body() user: UsersDto) {
    return this.userService.createUser(user);
  }
}
