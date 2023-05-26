import { Policy } from "./policy";

import { AuthContext } from "../models/auth-context.model";

export class AnonymousOnlyPolicy implements Policy {
    async authorizeAsync(authContext: AuthContext): Promise<void> {
        if (!authContext.authResult.shouldProceed)
            return;
        if (authContext.isAuthenticated)
            authContext.authResult.deny()
        else authContext.authResult.success();
    }
}