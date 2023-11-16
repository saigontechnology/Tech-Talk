import { Axios, AxiosResponse } from "axios";
import { InjectionKey } from "vue";

export interface MovieDTO {
  id: string;
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  vote_average: number;
  vote_count: number;
}

export interface MoviePagination<T> {
  page: number;
  results: T[];
  total_pages: number;
  total_results: number;
}

export const ACCESS_TOKEN =
  "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI1ZDViN2QwYTQyZDY1MmYwNWUxMWNmZGZiYjY2NGUzMyIsInN1YiI6IjY1MDdiNmViNDJkOGE1MDEzODlhZDI0OCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.4t51Xmpw3zk_8ZnWB5zMh--effatAQpoyAi4i8heK8I";

export class MovieService {
  axios = new Axios({
    baseURL: "https://api.themoviedb.org/3/movie",
    responseType: "json",
    headers: {
      accept: "application/json",
      Authorization: `Bearer ${ACCESS_TOKEN}`,
    },
    transformResponse: data => JSON.parse(data)
  });

  getPopular(): Promise<MovieDTO[]> {
    return this.axios
      .get("/popular")
      .then(
        (res: AxiosResponse<MoviePagination<MovieDTO>>) => res.data.results
      );
  }

  getTopRate(): Promise<MovieDTO[]> {
    return this.axios
      .get("/top_rated")
      .then(
        (res: AxiosResponse<MoviePagination<MovieDTO>>) => res.data.results
      );
  }
}

export const MOVIE_SERVICE_TOKEN: InjectionKey<MovieService> =
  Symbol("Movie_Service");
