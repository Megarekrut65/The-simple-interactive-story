<script setup>
import { RouterView, useRouter } from 'vue-router'
import { ref } from "vue"
import { logout, subscribeAuthChange } from './js/firebase/auth';
import LocalizedLink from './components/l10n/LocalizedLink.vue';
import LanguageSelect from './components/l10n/LanguageSelect.vue';

const footerBackground = new URL('@/assets/images/boa-background.png', import.meta.url).href;

const isLogin = ref(false), username = ref("");
const router = useRouter();

subscribeAuthChange((user) => {
  isLogin.value = user ? true : false;

  if (user) {
    username.value = user.displayName;
  }
});

const logoutBtn = () => {
  logout();
  router.go();
};

</script>

<template>
  <header class="navigation fixed-top" style="padding-bottom: 0; padding-top: 0;">
    <nav class="navbar navbar-expand-lg navbar-dark" style="padding-bottom: 0; padding-top: 0;">
      <LocalizedLink class="navbar-brand" to="/"><img class="logo-img" src="./assets/images/logo.png" alt="Logo">
      </LocalizedLink>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navigation"
        aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse text-center" id="navigation">
        <ul class="navbar-nav ml-auto">
          <li class="nav-item active">
            <LocalizedLink class="nav-link" to="">{{ $t('home') }}</LocalizedLink>
          </li>
          <li class="nav-item">
            <LocalizedLink class="nav-link" to="/">{{ $t('news') }}</LocalizedLink>
          </li>
          <li class="nav-item">
            <LocalizedLink class="nav-link" to="/">{{ $t('about') }}</LocalizedLink>
          </li>
          <li class="nav-item">
            <LocalizedLink class="nav-link" to="/">{{ $t('contacts') }}</LocalizedLink>
          </li>
          <li class="nav-item">
            <LanguageSelect></LanguageSelect>
          </li>
          <li class="nav-item" v-if="isLogin">
            <div class="dropdown">
              <a class="btn dropdown-toggle text-white" href="#" role="button" id="dropdownMenuLink"
                style="border:none;" data-bs-toggle="dropdown" aria-expanded="false">
                {{ username }}
              </a>

              <ul class="dropdown-menu" aria-labelledby="dropdownMenuLink" style="background-color: #07085d;">
                <li>
                  <LocalizedLink class="dropdown-item custom-item" to="account">{{ $t('account') }}</LocalizedLink>
                </li>
                <li>
                  <LocalizedLink class="dropdown-item custom-item" to="editor/new">{{ $t('createStory') }}
                  </LocalizedLink>
                </li>
                <li><a class="dropdown-item custom-item" href="#" @click="logoutBtn">{{ $t('logout') }}</a>
                </li>
              </ul>
            </div>
          </li>
          <li class="nav-item" v-else>
            <LocalizedLink class="nav-link" to="auth">{{ $t('signInUp') }}</LocalizedLink>
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
            <h4 class="text-white mb-5">{{ $t('help') }}</h4>
            <ul class="list-unstyled">
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('home') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('news') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('about') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('contacts') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('support') }}</LocalizedLink>
              </li>
            </ul>
          </div>
          <div class="col-md-3 col-6">
            <h4 class="text-white mb-5">{{ $t('story') }}</h4>
            <ul class="list-unstyled">
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('createStory') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('myStories') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('popularStories') }}</LocalizedLink>
              </li>
              <li>
                <LocalizedLink to="/" class="text-light d-block mb-3">{{ $t('account') }}</LocalizedLink>
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