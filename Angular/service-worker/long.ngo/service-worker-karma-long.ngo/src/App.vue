<template>
  <router-view></router-view>
</template>

<script setup lang="ts">
import { provide } from "vue";
import { MOVIE_SERVICE_TOKEN, MovieService } from "./api";
import { FORCE_LOGIN_SERVICE_TOKEN, ForceLoginService } from "./api/force-login.service";

provide(MOVIE_SERVICE_TOKEN, new MovieService());
provide(FORCE_LOGIN_SERVICE_TOKEN, new ForceLoginService());
</script>


<style lang="scss" scoped></style>
