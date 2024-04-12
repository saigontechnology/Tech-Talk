import axiosClient from "./clientApi";
import axiosClientNews from "./clientNewsApi";

export const covidApi = {
  getSummary(params) {
    const url = "/all";
    return axiosClient.get(url, { params });
  },
  getTimelineOfWorld(params) {
    const url = `/historical/all`;
    return axiosClient.get(url, { params });
  },
  getAllContinent(params) {
    const url = "/continents";
    return axiosClient.get(url, { params });
  },

  getAllCountry(params) {
    const url = `/countries`;
    return axiosClient.get(url, { params });
  },
  getByCountry(country, params) {
    const url = `/historical/${country}`;
    return axiosClient.get(url, { params });
  },
  getSummaryOfCountry(country) {
    const url = `/countries/${country}`;
    return axiosClient.get(url);
  },
};

export const newsApi = {
  getAllNews(params) {
    const url = "/articles";
    return axiosClientNews.get(url, { params });
  },
  getAllHotNews(params) {
    const url = "/top-headlines/sources";
    return axiosClientNews.get(url, { params });
  },
  getHotNews(params) {
    const url = "/top-headlines";
    return axiosClientNews.get(url, { params });
  },
};
