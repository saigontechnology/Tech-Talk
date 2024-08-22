import { Module } from '@nestjs/common';
import { UsersService } from './users.service';
import { UsersController } from './users.controller';
import { PostsModule } from '../posts/posts.module';

@Module({
  providers: [UsersService],
  controllers: [UsersController],
})
export class UsersModule {}
