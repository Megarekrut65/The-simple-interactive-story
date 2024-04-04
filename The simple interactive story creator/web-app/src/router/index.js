import { ifAuthenticated } from '@/js/firebase/auth'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('../views/HomeView.vue')
    },
    {
      path: '/auth',
      name: 'auth',
      component: () => import('../views/AuthView.vue')
    },
    {
      path: '/account',
      name: 'account',
      component: () => import('../views/AccountView.vue'),
      beforeEnter: ifAuthenticated
    },
    {
      path: '/editor/:storyId',
      name: 'editor',
      component: () => import('../views/StoryEditorView.vue'),
      beforeEnter: ifAuthenticated
    },
    {
      path: '/editor/new',
      name: 'new-editor',
      component: () => import('../views/NewStoryEditorView.vue'),
      beforeEnter: ifAuthenticated
    }
  ],
  scrollBehavior(to, from, savedPosition) {
    if(to == from) return savedPosition;

    return { top: 0 }
  },
})

export default router
