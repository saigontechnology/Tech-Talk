import { AuthContext } from "../models/auth-context.model";

export interface Policy {
    authorizeAsync(authContext: AuthContext): Promise<void>;
}