import { Module } from '@nestjs/common';
import { HttpModule } from '@nestjs/axios';
import { PassportModule } from '@nestjs/passport';
import { Auth0Strategy } from './auth0.strategy';
import { AuthController } from './auth.controller';
import { AuthService } from './auth.service';

@Module({
  imports: [PassportModule.register({ defaultStrategy: 'auth0' }), HttpModule],
  providers: [Auth0Strategy, AuthService],
  controllers: [AuthController],
  exports: [AuthService],
})
export class AuthModule {}
