<script setup>
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { ref } from "vue"
import { logout, subscribeAuthChange } from './js/firebase/auth';

const footerBackground = new URL('@/assets/images/boa-background.png', import.meta.url).href;

const isLogin = ref(false), username = ref("");
const router = useRouter();

subscribeAuthChange((user) => {
  isLogin.value = user ? true : false;

  if (user) {
    username.value = user.displayName;
  }
});

const logoutBtn = ()=>{
  logout();
  router.go();
};

</script>

<template>
  <header class="navigation fixed-top" style="padding-bottom: 0; padding-top: 0;">
    <nav class="navbar navbar-expand-lg navbar-dark" style="padding-bottom: 0; padding-top: 0;">
      <RouterLink class="navbar-brand" to="/"><img class="logo-img" src="./assets/images/logo.png" alt="Logo">
      </RouterLink>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navigation"
        aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse text-center" id="navigation">
        <ul class="navbar-nav ml-auto">
          <li class="nav-item active">
            <RouterLink class="nav-link" to="/">Home</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" to="/">News</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" to="/">About</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" to="/">Contact</RouterLink>
          </li>
          <li class="nav-item" v-if="isLogin">
            <div class="dropdown">
              <a class="btn dropdown-toggle text-white" href="#" role="button" id="dropdownMenuLink" style="border:none;"
                data-bs-toggle="dropdown" aria-expanded="false">
                {{ username }}
              </a>

              <ul class="dropdown-menu" aria-labelledby="dropdownMenuLink" style="background-color: #07085d;">
                <li><RouterLink class="dropdown-item" style="color: #fff;" to="/account">Account</RouterLink></li>
                <li><RouterLink class="dropdown-item" style="color: #fff;" to="/">Create story</RouterLink></li>
                <li><a class="dropdown-item" style="color: #fff;" href="#" @click="logoutBtn">Logout</a></li>
              </ul>
            </div>
          </li>
          <li class="nav-item" v-else>
            <RouterLink class="nav-link" to="/auth">Sing in/up</RouterLink>
          </li>

        </ul>
      </div>
    </nav>
  </header>

  <RouterView />

  <footer class="position-relative text-center" :style="{ backgroundImage: 'url(' + footerBackground + ')' }">
    <div class="section">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-md-3 col-6">
            <h4 class="text-white mb-5">Help</h4>
            <ul class="list-unstyled">
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Home</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">News</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">About us</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Contact</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Support</RouterLink>
              </li>
            </ul>
          </div>
          <div class="col-md-3 col-6">
            <h4 class="text-white mb-5">Story</h4>
            <ul class="list-unstyled">
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Create new story</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">My stories</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Popular stories</RouterLink>
              </li>
              <li>
                <RouterLink to="/" class="text-light d-block mb-3">Account</RouterLink>
              </li>

            </ul>
          </div>
          <div class="col-md-6 logo-container mt-md-0 mt-5">
            <img src="@/assets/images/logo.png" class="company-logo" alt="Game logo">

            <img src="@/assets/images/by.png" class="by-logo" alt="By">

            <img src="@/assets/images/boa-games.png" class="company-logo" alt="Company logo">
          </div>
        </div>
      </div>
    </div>
  </footer>
</template>