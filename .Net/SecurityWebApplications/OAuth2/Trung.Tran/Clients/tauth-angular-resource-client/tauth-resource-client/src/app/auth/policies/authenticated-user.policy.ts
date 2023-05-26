import { Policy } from "./policy";

import { AuthContext } from "../models/auth-context.model";

export class AuthenticatedUserPolicy implements Policy {
    async authorizeAsync(authContext: AuthContext): Promise<void> {
        if (!authContext.authResult.shouldProceed)
            return;
        if (authContext.isAuthenticated)
            authContext.authResult.success()
        else authContext.authResult.unauthorize();
    }
}