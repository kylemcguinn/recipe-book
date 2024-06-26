import { createMemoryHistory, createRouter } from 'vue-router'

import HomePage from './components/HomePage/HomePage.vue'
import RecipesPage from './components/RecipesPage/RecipesPage.vue'

const routes = [
  { path: '/', component: HomePage },
  { path: '/recipes', component: RecipesPage },
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
})

export default router