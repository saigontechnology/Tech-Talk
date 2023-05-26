const IdentityService = () => {
    const config = {
        authority: "https://localhost:44339/",
        client_id: "resource-client-js-plain-id",
        redirect_uri: "http://127.0.0.1:52330/callback.html",
        response_type: "id_token token",
        scope: "openid profile address roles resource_api.full",
        post_logout_redirect_uri: "http://127.0.0.1:52330",
        automaticSilentRenew: true,
        silent_redirect_uri: 'http://127.0.0.1:52330/silent-refresh.html'
    };
    const manager = new Oidc.UserManager(config);
    manager.events.addSilentRenewError(err => {
        console.log(err);
    });
    let _token = null;

    const getUser = (handler) => {
        manager.getUser().then(handler);
    }

    const login = () => {
        manager.signinRedirect();
    }

    const logout = () => {
        _token = null;
        manager.signoutRedirect();
    }

    const authenticateUser = () => {
        manager.getUser().then(user => {
            if (!user) {
                login();
            } else {
                _token = user.access_token;
            }
        });
    }

    const forbid = () => {
        window.location = '/access-denied.html#' + new Date().getTime();
    }

    const accessToken = () => {
        return _token;
    };

    return {
        getUser,
        login,
        logout,
        authenticateUser,
        forbid,
        accessToken
    }
};