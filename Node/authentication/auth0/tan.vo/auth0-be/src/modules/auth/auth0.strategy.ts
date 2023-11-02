import { passportJwtSecret } from 'jwks-rsa';
import { ExtractJwt, Strategy, VerifiedCallback } from 'passport-jwt';

import { Injectable, UnauthorizedException } from '@nestjs/common';
import { PassportStrategy } from '@nestjs/passport';

@Injectable()
export class Auth0Strategy extends PassportStrategy(Strategy, 'auth0') {
  constructor() {
    super({
      secretOrKeyProvider: passportJwtSecret({
        cache: true,
        rateLimit: false,
        jwksUri: `https://universal-login.au.auth0.com/.well-known/jwks.json`,
      }),
      jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
      audience: 'http://localhost:8000',
      issuer: `https://universal-login.au.auth0.com/`,
      algorithms: ['RS256'],
    });
  }

  validate(payload: any, done: VerifiedCallback) {
    if (!payload) {
      done(new UnauthorizedException(), false);
    }

    return done(null, payload);
  }
}
