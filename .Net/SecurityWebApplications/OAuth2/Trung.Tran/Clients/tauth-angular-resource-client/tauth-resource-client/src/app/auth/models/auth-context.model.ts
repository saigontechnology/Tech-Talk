import { User } from "oidc-client";

import { AuthResult } from "./auth-result.model";

export class AuthContext {
    constructor(public isAuthenticated: boolean, public user: User | null,
        public authResult: AuthResult = new AuthResult()) {
    }
}