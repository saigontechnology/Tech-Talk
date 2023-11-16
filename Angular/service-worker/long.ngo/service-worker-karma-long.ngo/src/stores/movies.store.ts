import { MOVIE_SERVICE_TOKEN, MovieDTO } from "@/api";
import { defineStore } from "pinia";
import { computed, inject, ref } from "vue";

export interface State {
  popular: MovieDTO[];
  topRate: MovieDTO[];
  loading: boolean;
}

export const useMovieStore = defineStore("movies", () => {
  //Service DI
  const movieService = inject(MOVIE_SERVICE_TOKEN);

  // State
  const state = ref<State>({
    popular: [],
    topRate: [],
    loading: false,
  });

  //Getter
  const getPopular = computed(() => state.value.popular);
  const getIsLoading = computed(() => state.value.loading);
  const getTopRate = computed(() => state.value.topRate);

  // const getters = {
  //   getPopular: computed(() => state.value.popular),
  //   getIsLoading: computed(() => state.value.loading),
  //   getTopRate: computed(() => state.value.topRate),
  // };

  //Actions
  const setLoading = (loading: boolean) => (state.value.loading = loading);
  const setPopular = (popular: MovieDTO[]) => (state.value.popular = popular);
  const setTopRate = (topRate: MovieDTO[]) => (state.value.topRate = topRate);

  //Effect
  const actions = {
    dispatchLoadPopular: async () => {
      try {
        setLoading(true);
        const movies = await movieService.getPopular();
        setPopular(movies);
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    },
    disPatchLoadTopRate: async () => {
      try {
        setLoading(true);
        const movies = await movieService.getTopRate();
        setTopRate(movies);
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    },
  };

  return {
    state,
    actions,
    getPopular,
    getIsLoading,
    getTopRate
  };
});
