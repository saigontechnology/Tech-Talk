import { Body, Controller, Post, Res } from '@nestjs/common';
import { Response } from 'express';
import { AuthService } from './auth.service';
import { AuthDto, RefreshTokenDto } from './auth0/auth0.dto';

@Controller('auth')
export class AuthController {
  constructor(private authService: AuthService) {}

  @Post('/login')
  async login(
    @Body() auth: AuthDto,
    @Res({ passthrough: true }) response: Response,
  ): Promise<any> {
    const { access_token, refresh_token } = await this.authService.login(auth);
    response.send({
      message: 'Login was successful!',
      access_token,
      refresh_token,
    });
  }

  @Post('/refresh-token')
  async refreshToken(@Body() data: RefreshTokenDto): Promise<any> {
    return await this.authService.refreshToken(data);
  }
}
