class BaseSW {
  swInstance = self as unknown as ServiceWorkerGlobalScope;

  onInstall: (ev: ExtendableEvent) => void;
  onActivate: (ev: ExtendableEvent) => void;
  onFetch: (ev: FetchEvent) => void;
  onMessage: (ev: ExtendableMessageEvent) => void;
  onBackgroundSync: (ev: any) => void;
  onPushNotification: (ev: any) => void;

  init(): void {
    this.swInstance.addEventListener("install", this.onInstall);
    this.swInstance.addEventListener("activate", this.onActivate);
    this.swInstance.addEventListener("fetch", this.onFetch);
    this.swInstance.addEventListener("message", this.onMessage);
    this.swInstance.addEventListener("sync", this.onBackgroundSync);
    this.swInstance.addEventListener("push", this.onPushNotification);
  }
}
class CacheSW extends BaseSW {
  version = "1.0.0";
  staticCacheName = "static-cache" + this.version;
  requestCacheName = "request-cache" + this.version;
  backgroundSyncMap = new Map<string, Request>()

  assets = ["/index.html"];

  override onInstall = (ev: ExtendableEvent) => {
    //Immediate active new sw
    this.swInstance.skipWaiting();

    //Perform cache all static file
    ev.waitUntil(
      (async () => {
        //Init Cache
        const cacheStoreInstance = await caches.open(this.staticCacheName);
        console.log(
          `>>>${this.staticCacheName} init success`,
          cacheStoreInstance
        );

        // url string key
        // const urlString = '/movie-background?id=1';

        // Url Object
        // const urlObject = new URL('http://127.0.0.1:5173/movie-background?id=2');

        // Request Object
        const reqObject = new Request(
          "http://127.0.0.1:5173/movie-background?id=3"
        );

        //Perform fetch and put on cache
        await cacheStoreInstance.add(reqObject);

        // Cache static
        await cacheStoreInstance
          .addAll(this.assets)
          .then(() => console.log("cache has been update"))
          .catch((error) => console.log("cache update failed", error));
      })()
    );
  };

  //on Activate
  override onActivate = (ev: ExtendableEvent) => {
    //Active new cache and remove old cache

    ev.waitUntil(
      (async () => {
        const cacheKeys = await caches.keys();

        await Promise.all(
          cacheKeys
            .filter((key) => key !== this.staticCacheName)
            .map((key) => caches.delete(key))
        );
      })()
    );
  };

  //On Fetch
  override onFetch = (ev: FetchEvent) => {
    //Skip for request chrome extension
    if (ev.request.url.startsWith("chrome-extension")) {
      return;
    }

    ev.respondWith(
      (async () => {
        // Cache first, falling back to network
        // return this.cacheFirst(ev);

        //Network first, falling back to cache
        return this.networkFirst(ev)

        //Stale-while-revalidate
        // return this.staleWhileRevalidate(ev);

        //Cache only
        // return this.onlyCache(ev);

        //do background job
        // return this.doBackgroundJob(ev);
      })()
    );
  };

  //On Message
  override onMessage = (ev: ExtendableMessageEvent) => {
    //Client that sent message
    ev.waitUntil(
      (async () => {
        const currentCLient = ev.source as Client;
        const allMatchClient = (
          await this.swInstance.clients.matchAll()
        ).filter((client) => client.id !== currentCLient.id);

        //Broadcast to all Client
        allMatchClient.forEach((client) => {
          client.postMessage(ev.data);
        });
      })()
    );
  };

  //Background Sync
  override onBackgroundSync = (ev: any) => {
    // Fallback network request on this
    // console.log("background-sync", ev.tag)

    
  };

  override  onPushNotification = (ev: any) => {
    console.log("push-noti", ev.data)
  };

  async cacheFirst(ev: FetchEvent): Promise<Response> {
    const matchedCache = await caches.match(ev.request);
    // console.log(ev.request);
    //If has matched cache then return
    if (matchedCache) return matchedCache;
    // Clone the request. A request is a stream and
    // can only be consumed once. Since we are consuming this
    // once by cache and once by the browser for fetch
    const fetchRequest = ev.request.clone();
    const fetchResponse = await fetch(fetchRequest);

    // If Fetch failed return failed res
    if (fetchResponse.status === 200) {
      const requestCacheInstance = await caches.open(this.requestCacheName);
      await requestCacheInstance.put(ev.request, fetchResponse.clone());
    }

    return fetchResponse;
  }

  async networkFirst(ev: FetchEvent): Promise<Response> {
    try {
      const fetchResponse = await fetch(ev.request);
      const cacheInstance = await caches.open(this.requestCacheName);

      await cacheInstance.put(ev.request.clone(), fetchResponse.clone());

      return fetchResponse;
    } catch (error) {
      // if network failed fallback to cache
      return await caches.match(ev.request);
    }
  }

  async staleWhileRevalidate(ev: FetchEvent): Promise<Response> {
    if (ev.request.destination === "image") {
      const cacheInstance = await caches.open(this.requestCacheName);
      const matchedCache = await cacheInstance.match(ev.request);
      const fetchResponse = await fetch(ev.request);

      await cacheInstance.put(ev.request.clone(), fetchResponse.clone());

      return matchedCache ?? fetchResponse;
    }
  }

  async onlyCache(ev: FetchEvent): Promise<Response> {
    const url = new URL(ev.request.url);
    const isPreCachedRequest = ["movie-background"].includes(url.pathname);
    const cacheInstance = await caches.open(this.staticCacheName);

    if (isPreCachedRequest) {
      return cacheInstance.match(ev.request);
    }

    // If no pre-cache resource -> Network only strategy
    return;
  }

  // async doBackgroundJob(ev: FetchEvent): Promise<Response> {
  //  try {
  //   const fetchResponse = await fetch(ev.request);
  //   return fetchResponse;
  //  } catch (error) {
  //    this.backgroundSyncMap.set(ev.request.url, ev.request);
  //    const registration: any = await this.swInstance.registration;
  //     await registration.sync.register("bg-sync");

  //     return await caches.match(ev.request);
  //  }
  // };
}

const CacheSWInstance = new CacheSW();
CacheSWInstance.init();
