const CACHE_ASSETS_NAME = 'cache-assets-v1';
const CACHE_NETWORK_NAME = 'cache-network-v1';

self.addEventListener('install', (event) => {
	// pre-cache assets need for run web app offline
	// event.waitUntil(
	// 	caches.open(CACHE_ASSETS_NAME).then((cache) =>{
	// 		return cache.addAll([
	// 			'index.html',
	// 			'bundled.js',
	// 			'favicon.ico',
	// 			'logo192.png',
	// 			'manifest.json',
	// 			'vender-main.chunk.js',
	// 			'main.chunk.js',
	// 		])
	// 	})
	// )
	console.log('installing service worker');
});

self.addEventListener('activate', (event) => {
	// console.log('activating service worker');
});

self.addEventListener('fetch', (event) => {
	const request = event.request;
	event.respondWith(
		caches
			.open(CACHE_NETWORK_NAME)
			.then((cache) => {
				return cache.match(request).then((response) => {
	// 				if (response) {
	// 					// If there is an entry in the cache for event.request, then response will be defined
	// 					// and we can just return it. Note that in this example, only font resources are cached.
	// 					console.log(' Found response in cache:', response);

	// 					return response;
	// 				}
	//				// HTML
	// 				// if (request.headers.get('Accept').includes('text/html')) {
	// 				// 	// Handle HTML files...
	// 				// 	return;
	// 				// }
	// 				// CSS & JavaScript
	// 				// if (
	// 				// 	request.headers.get('Accept').includes('text/css') ||
	// 				// 	request.headers.get('Accept').includes('text/javascript')
	// 				// ) {
	// 				// 	// Handle CSS and JavaScript files...
	// 				// 	return;
	// 				// }

	// 				// // Images
	// 				// if (request.headers.get('Accept').includes('image')) {
	// 				// 	// Handle images...
	// 				// 	return;
	// 				// }

					return fetch(request.clone()).then((response) => {
						cache.put(request, response.clone());
						return response;
					});
				});
			})
			.catch((error) => {
				console.log(error);
			}),
	);
});
