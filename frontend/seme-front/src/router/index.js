import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import MapView from '../views/mapView/MapView.vue'
import MapViewz from '../views/mapView/MapView-0.vue'
import MyBaiduMap from '../components/MyBaiduMap.vue'
import HousingAnalysisView from '@/views/housingAnalysisView/HousingAnalysisView.vue'
import BusinessAnalysisView from '@/views/businessAnalysisView/BusinessAnalysisView.vue'
import FavoritesView from '@/views/favoritesView/FavoritesView.vue'
import LoginView from '@/views/userView/LoginView.vue'
import RegisterView from '@/views/userView/RegisterView.vue'
import AgentsView from '@/views/agentsView/AgentsView.vue'
import CommunityView from '@/views/communityView/CommunityView.vue'
import LocationView from '@/views/agentsView/LocationView.vue'

const routes = [
  {
    path: '/',
    name: 'LoginView',
    component: LoginView
  },
  {
    path: '/RegisterView',
    name: 'RegisterView',
    component: RegisterView
  },
  {
    path: '/HomeView',
    name: 'HomeView',
    component: HomeView
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  },
  {
    path:'/MapView',
    name:'MapView',
    component:MapView
  },
  {
    path:'/MapViewz',
    name:'MapViewz',
    component:MapViewz
  },
  {
    path:'/MyBaiduMap',
    name:'MyBaiduMap',
    component:MyBaiduMap
  },
  {
    path:'/HousingAnalysisView',
    name:'HousingAnalysisView',
    component:HousingAnalysisView
  },
  {
    path:'/BusinessAnalysisView',
    name:'BusinessAnalysisView',
    component:BusinessAnalysisView
  },
  {
    path:'/FavoritesView',
    name:'FavoritesView',
    component:FavoritesView
  },
  {
    path:'/CommunityView',
    name:'CommunityView',
    component:CommunityView
  },
  {
    path:'/AgentsView',
    name:'AgentsView',
    component:AgentsView
  },
  {
    path:'/LocationView',
    name:'LocationView',
    component:LocationView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
