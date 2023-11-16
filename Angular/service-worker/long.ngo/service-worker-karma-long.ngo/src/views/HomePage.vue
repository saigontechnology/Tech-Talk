<template>
  <vs-navbar center-collapsed>
    <template #left>
      <h1 color="primary">Dark movie</h1>
    </template>

    <template #right>
      <vs-button flat @click="sayHiToAllClients()">Say Hi </vs-button>
      <vs-button flat @click="login()">Force login</vs-button>
    </template>
  </vs-navbar>
  <div class="movie-grid">
    <vs-card v-for="movie in popularMovies" type="1">
      <template #title>
        <h3>{{ movie.title }}</h3>
      </template>
      <template #img>
        <img
          :src="'https://image.tmdb.org/t/p/original' + movie.backdrop_path"
          alt="backdrop"
        />
      </template>
      <template #text>
        <p>
          {{ movie.overview }}
        </p>
      </template>
      <template #interactions>
        <vs-button color="dark" class="icon-button">
          <i class="bx bx-star"></i>
          <span class="span">
            {{ movie.vote_average }}
          </span>
        </vs-button>
      </template>
    </vs-card>
  </div>
</template>
<script lang="ts" setup>

import { FORCE_LOGIN_SERVICE_TOKEN, ForceLoginService } from "@/api/force-login.service";
import { useMovieStore } from "@/stores";
import { storeToRefs } from "pinia";
import { inject, onMounted } from "vue";

const movieStore = useMovieStore();
const popularMovies = storeToRefs(movieStore).getPopular;
const sw = inject<ServiceWorker>("SW");
const forceLogin = inject<ForceLoginService>(
  FORCE_LOGIN_SERVICE_TOKEN
);

const sayHiToAllClients = () => {
  sw.postMessage("Message From Home Page");
};

const login = async () => {
  await forceLogin
    .login()
    .then(() => console.log("post success"))
    .catch(async () => {
      const registration: any = await navigator.serviceWorker.ready;
      registration.sync.register('bg-tag')
    })
};

onMounted(async () => {
  await movieStore.actions.dispatchLoadPopular();

  navigator.serviceWorker.addEventListener(
    "message",
    (ev: MessageEvent<string>) => {
      window.alert(ev.data);
    }
  );
});
</script>

<style lang="scss" scoped>
.movie-grid {
  display: flex;
  flex-direction: row;
  gap: 10px;
  flex-wrap: wrap;
  justify-content: center;

  .icon-button {
    font-size: 16px;
  }
}
</style>
@/api/force-login.service