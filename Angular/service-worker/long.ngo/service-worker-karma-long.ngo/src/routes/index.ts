import { createRouter, createWebHistory } from "vue-router";
export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/home-page",
      component: () => import("@/views/HomePage.vue"),
      
    },
    {
      path: "/login",
      component: () => import("@/views/LoginPage.vue"),
    },
    {
      path: "",
      redirect: '/home-page'
    }
  ],
});
