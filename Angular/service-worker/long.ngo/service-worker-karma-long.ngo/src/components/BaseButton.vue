
<template>
  <button @click="clickTracker()" :style="{ width: '200px' }">
    <slot>123</slot>
  </button>
</template>

<script setup lang="ts">
const props = defineProps<{
  clickTracker: Function;
}>();
</script>