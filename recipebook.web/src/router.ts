import { createWebHistory, createRouter } from 'vue-router'

import HomePage from './components/HomePage/HomePage.vue'
import RecipesPage from './components/RecipesPage/RecipesPage.vue'
import SettingsPage from './components/SettingsPage/SettingsPage.vue'

const routes = [
  { path: '/', component: HomePage },
  { path: '/recipes', component: RecipesPage },
  { path: '/settings', component: SettingsPage },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router