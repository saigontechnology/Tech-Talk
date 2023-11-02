import axios from "axios";

const instance = axios.create({
  baseURL: "http://localhost:8000",
});

instance.interceptors.request.use((config) => {
  const accessToken = localStorage.getItem("accessToken");
  config.headers["Authorization"] = `Bearer ${accessToken}`;
  return config;
});

instance.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequet = error.config;
    originalRequet.isRetry = false;
    if (error.response.status === 401) {
      if (!originalRequet.isRetry) {
        originalRequet.isRetry = true;
        const accessToken = await refreshAccessToken();
        localStorage.setItem("accessToken", accessToken);
        originalRequet.headers.Authorization = `Bearer ${accessToken}`;
        return instance(originalRequet);
      } else {
        window.location.href = "/";
      }
    } else {
      return Promise.reject(error);
    }
  }
);

const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  const { data } = await instance.post("/auth/refresh-token", { refreshToken });
  return data.access_token;
};

export default instance;
