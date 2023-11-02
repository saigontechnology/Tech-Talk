import { HttpService } from '@nestjs/axios';
import { HttpException, Injectable } from '@nestjs/common';
import { AuthDto, RefreshTokenDto } from './auth0/auth0.dto';
import { catchError, firstValueFrom, map } from 'rxjs';

@Injectable()
export class AuthService {
  constructor(private readonly httpService: HttpService) {}
  async login(auth: AuthDto) {
    const url = `https://universal-login.au.auth0.com/oauth/token`;
    const data = {
      grant_type: 'password',
      username: auth.email,
      password: auth.password,
      audience: 'http://localhost:8000',
      client_id: '4pY8drPRWh8L83xVew9tmTlCp7yLDUqN',
      client_secret:
        's8oZ7r81VNe_HjOJ6yDkP7hfMkhaJEhNG2P2svylmwkuU9xU6XdWC_UBsIV7XQHr',
      scope: 'offline_access',
    };
    const { access_token, refresh_token } = await firstValueFrom(
      this.httpService
        .post(url, data, {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
          },
        })
        .pipe(
          map((response: any) => {
            return response.data;
          }),
          catchError((e: any) => {
            throw new HttpException(e.response.data, e.response.status);
          }),
        ),
    );
    return {
      access_token,
      refresh_token,
    };
  }

  async refreshToken(data: RefreshTokenDto): Promise<any> {
    const url = `https://universal-login.au.auth0.com/oauth/token`;
    const req = {
      grant_type: 'refresh_token',
      client_id: '4pY8drPRWh8L83xVew9tmTlCp7yLDUqN',
      client_secret:
        's8oZ7r81VNe_HjOJ6yDkP7hfMkhaJEhNG2P2svylmwkuU9xU6XdWC_UBsIV7XQHr',
      refresh_token: data.refreshToken,
    };
    const { access_token } = await firstValueFrom(
      this.httpService
        .post(url, req, {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
          },
        })
        .pipe(
          map((response: any) => {
            return response.data;
          }),
          catchError((e: any) => {
            throw new HttpException(e.response.data, e.response.status);
          }),
        ),
    );
    return { access_token };
  }
}
