import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import axios from "../axios";

function Authentication() {
  const [hasLoggedIn, setHasLoggedIn] = useState(false);
  const [userInfo, setUserInfo] = useState(null);
  const {
    register,
    handleSubmit,
    formState: { isValid },
  } = useForm();

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      setHasLoggedIn(true);
      getInfo();
    }
  }, []);

  const handleLogin = async (user) => {
    const { data } = await axios.post(`/auth/login`, user);
    const { access_token, refresh_token } = data;
    localStorage.setItem("accessToken", access_token);
    localStorage.setItem("refreshToken", refresh_token);
    getInfo();
    setHasLoggedIn(true);
  };

  const getInfo = async () => {
    try {
      const { data } = await axios.get("/users");
      setUserInfo(data);
    } catch (e) {
      console.log(e);
    }
  };

  const handleLogOut = () => {
    localStorage.clear();
    setHasLoggedIn(false);
  };

  return (
    <div className="h-screen background">
      <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] p-8">
        {hasLoggedIn ? (
          <div className="bg-white p-8 shadow-xl text-center">
            <div className="text-2xl">{userInfo?.name}</div>
            <div className="text-2xl mt-8">{userInfo?.email}</div>
            <button
              className="mt-8 ml-4 border-2 uppercase px-6 py-2 bg-white  border-[#8dc63f] text-[#8dc63f]"
              onClick={handleLogOut}
            >
              Log Out
            </button>
          </div>
        ) : (
          <form
            onSubmit={handleSubmit(handleLogin)}
            className="shadow-xl p-8 bg-white"
          >
            <div className="text-3xl uppercase font-semibold tracking-wider text-center">
              Resource Owner Password
            </div>
            <div className="mt-6">
              <label htmlFor="email" className="block font-medium">
                <span className="text-[#8dc63f]">*</span>Email
              </label>
              <input
                {...register("email", { required: true })}
                placeholder="Enter your email"
                id="email"
                className="block w-full mt-2 p-4 rounded border border-gray-100 outline-[#8dc63f]"
              />
            </div>
            <div className="mt-6">
              <label htmlFor="password" className="block font-medium">
                <span className="text-[#8dc63f]">*</span>Password
              </label>
              <input
                {...register("password", { required: true })}
                type="password"
                placeholder="Enter your password"
                id="password"
                className="block w-full mt-2 p-4 rounded border border-gray-100 outline-[#8dc63f]"
              />
            </div>
            <div className="mt-6">
              <button
                className={`w-full p-6 uppercase rounded text-white font-bold leading-4 text-center focus:scale-95 transition-all ${
                  isValid ? "bg-[#8dc63f]" : "bg-[#8dc63f]/50"
                } `}
                disabled={!isValid}
              >
                Log In
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}

export default Authentication;
