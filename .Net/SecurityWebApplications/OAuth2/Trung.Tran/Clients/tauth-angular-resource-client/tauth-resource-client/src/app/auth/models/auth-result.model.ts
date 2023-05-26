export class AuthResult {
    private _success?: boolean;
    get isSuccess(): boolean {
        return this._success || false;
    }

    private _unauthorized: boolean;
    get unauthorized() {
        return this._unauthorized;
    }

    private _accessDenied: boolean;
    get accessDenied() {
        return this._accessDenied;
    }

    constructor(public shouldProceed: boolean = true) {
        this._unauthorized = false;
        this._accessDenied = false;
    }

    success() {
        this._success = true;
        this._accessDenied = false;
        this._unauthorized = false;
    }

    deny() {
        this._success = false;
        this._unauthorized = false;
        this._accessDenied = true;
    }

    unauthorize() {
        this._success = false;
        this._accessDenied = false;
        this._unauthorized = true;
    }
}