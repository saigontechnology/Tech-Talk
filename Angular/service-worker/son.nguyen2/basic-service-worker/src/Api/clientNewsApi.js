import axios from "axios";

const axiosClientNews = axios.create({
  // baseURL: "https://newsapi.org/v2",
  baseURL: "https://article-json-server.herokuapp.com",
  headers: {
    "Content-Type": "application/json",
  },
});

axiosClientNews.interceptors.request.use(
  function (config) {
    return config;
  },
  function (error) {
    // Do something with request error
    return Promise.reject(error);
  }
);

axiosClientNews.interceptors.response.use(
  function (response) {
    return response.data;
  },
  function (error) {
    return Promise.reject(error);
  }
);

export default axiosClientNews;
