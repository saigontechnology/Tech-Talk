<template>
  <section class="page-container">
    <div class="form">
      <vs-input placeholder="User name" />
      <vs-input placeholder="Password" />
      <vs-button flat @click="clickSubmit()">Login</vs-button>
    </div>
  </section>
</template>
<script lang="ts" setup>
import { inject } from "vue";
const sw = inject<ServiceWorker>('SW');

const clickSubmit = () => {
  sw.postMessage('Message From Login Page')
}
</script>

<style scoped lang="scss">
.page-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  align-items: center;
  justify-content: center;

  .form {
    width: 500px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 10px;
    padding: 20px;
    background-color: #28282863;
  }
}
</style>
