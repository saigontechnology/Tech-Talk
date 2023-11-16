import { createApp } from "vue";
import "./style.scss";
import App from "./App.vue";
import Vuesax from 'vuesax-alpha'
import 'vuesax-alpha/dist/index.css'
import 'vuesax-alpha/theme-chalk/dark/css-vars.css'

import { router } from './routes';
import { createPinia } from "pinia";
import { provideSW } from "../registerServiceWorker";

const app = createApp(App);
const pinia = createPinia();

app.use(Vuesax);
app.use(router);
app.use(pinia);

provideSW(app).then(() => app.mount("#app"));


