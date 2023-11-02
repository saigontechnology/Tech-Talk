import { useAuth0 } from "@auth0/auth0-react";

function UniversalLogin() {
  const { loginWithRedirect, user, isAuthenticated, logout } = useAuth0();

  return (
    <div className="background h-screen">
      <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 text-center">
        <div className="w-[100px] mx-auto">
          <img
            src="./auth0-logo.png"
            className="w-full h-auto"
            alt="auth-logo"
          />
        </div>
        <div className="font-medium text-5xl text-white uppercase tracking-widest mt-8">
          Universal Login
        </div>
        {isAuthenticated ? (
          <div className="mt-8">
            <div className="text-2xl text-white">{user.name}</div>
            <div className="text-2xl text-white">{user.email}</div>
            <div>{user.age}</div>
            <button
              className="mt-8 border-2 uppercase border-white text-white px-6 py-2 transition-colors hover:bg-white hover:text-[#008552] font-bold"
              onClick={() => logout()}
            >
              Log Out
            </button>
          </div>
        ) : (
          <button
            className="mt-8 border-2 uppercase border-white text-white px-6 py-2 transition-colors hover:bg-white hover:text-[#008552] font-bold"
            onClick={() => loginWithRedirect()}
          >
            Log In
          </button>
        )}
      </div>
    </div>
  );
}

export default UniversalLogin;
