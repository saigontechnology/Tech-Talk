
self.addEventListener("install", (evt: any) => {
  evt.waitUntil(
    (async () => {
      console.log(">>> Installed login page SW");
    })()
  );
});

self.addEventListener("message", (evt: any) => {
  evt.waitUntil(
    (async () => {
      const keys = await caches.keys();
      console.log(">>> read caches storage from login-page", keys);
    })()
  );
});
