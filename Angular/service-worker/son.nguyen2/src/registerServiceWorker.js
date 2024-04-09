export const registerServiceWorker = (scope = '/') => {
	if ('serviceWorker' in navigator) {
		navigator.serviceWorker
			.register('/service-worker.js', { scope })
			.then((registration) => {
				console.log('SW registered: ', registration);
			})
			.catch((registrationError) => {
				console.log('SW registration failed: ', registrationError);
			});
	}
};
