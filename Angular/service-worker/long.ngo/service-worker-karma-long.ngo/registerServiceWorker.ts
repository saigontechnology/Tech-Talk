import { App } from "vue";

export const registerSW = async (): Promise<void> => {
  if ("serviceWorker" in navigator) {
    const homePageSW = await navigator.serviceWorker.register(
      "./serviceWorker",
      { scope: "./home-page" }
    );
    const loginPageSW = await navigator.serviceWorker.register(
      "./loginServiceWorker",
      { scope: "./login" }
    );
    console.log(">>>Home page service worker registered", homePageSW);
    console.log(">>>Login page service worker registered ", loginPageSW);
    return;
  }

  console.log(">>> SW not supported");
};

export const unRegisterSW = async (): Promise<void> => {
  const registrations: readonly ServiceWorkerRegistration[] =
    await navigator.serviceWorker.getRegistrations();

  registrations?.forEach(async (reg) => await reg.unregister());
};

export const provideSW = async (app: App<Element>) => {
  await registerSW();

  app.provide("SW", navigator.serviceWorker.controller);
};
