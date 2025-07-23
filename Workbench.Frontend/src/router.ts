import { createRouter, createWebHistory } from 'vue-router'

import AccountDirectAccessApplication from '@/pages/AccountDirectAccessApplication.vue'
import AppListPage from '@/pages/AppListPage.vue'
import CallbackPage from '@/pages/CallbackPage.vue'
import CreateAppPage from '@/pages/CreateAppPage.vue'

const routes = [
  { path: '/', component: AppListPage },
  { path: '/create', name: 'create-app', component: CreateAppPage },
  {
    path: '/callback',
    name: 'callback',
    component: CallbackPage,
  },
  { path: '/apps/:id', component: AccountDirectAccessApplication },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
