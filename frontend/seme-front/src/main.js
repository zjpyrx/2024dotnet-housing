import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import BaiduMap from 'vue-baidu-map-3x'
import ElementPlus from 'element-plus'
import '../node_modules/element-plus/theme-chalk/index.css';
import * as ElementPlusIconsVue from '@element-plus/icons-vue';
import './assets/fonts/font.css'

const app = createApp(App);
app.use(router);
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component);
}

app.use(ElementPlus);
app.use(BaiduMap,{
    ak:'XpngTihSxTcAvh9nct8UIiv1R0QBEjbF',
});
app.use(store);
app.mount('#app');
