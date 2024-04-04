<script setup>
import { RouterLink, RouterView } from 'vue-router'

</script>

<template>
  <header class="navigation fixed-top" style="padding-bottom: 0; padding-top: 0;">
  <nav class="navbar navbar-expand-lg navbar-dark" style="padding-bottom: 0; padding-top: 0;">
    <RouterLink class="navbar-brand" to="/"><img class="logo-img" src="./assets/images/logo.png" alt="Logo"></RouterLink>
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
        <li class="nav-item">
          <RouterLink class="nav-link" to="/">Account</RouterLink>
        </li>
      </ul>
    </div>
  </nav>
</header>

  <RouterView />
</template>
