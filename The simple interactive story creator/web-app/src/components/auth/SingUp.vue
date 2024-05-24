<script setup>
import { ref } from 'vue'
import { createNewUser } from '@/js/firebase/auth';
import { useRouter } from 'vue-router';
import LoadingWindow from '../LoadingWindow.vue';
import { goNext } from '@/js/utilities/router-utility';

const router = useRouter();

const isLoading = ref(false);

const nickname = ref(""), email = ref(""), password = ref(""), passwordRepeat = ref("");
const error = ref("");

const onSubmit = () => {
    if (password.value != passwordRepeat.value) {
        error.value = "Passwords don't match!";
        return;
    }

    error.value = "";
    isLoading.value = true;


    createNewUser(nickname.value, email.value, password.value)
        .then(() => goNext(router))
        .catch(err => {
            console.log(err);
            isLoading.value = false;
            error.value = err;
        });
};

</script>

<template>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <div class="border border-dark p-3 m-lg-3 m-1">
        <h3>Sing up</h3>

        <form @submit="onSubmit" action="#" onsubmit="return false;">
            <input v-model.trim="nickname" type="text" name="name" placeholder="Nickname..." required minlength="2"
                class="form-control mb-4 shadow rounded-0" maxlength="1000">
            <input v-model.trim="email" type="email" name="email" placeholder="Email..." required minlength="4"
                class="form-control mb-4 shadow rounded-0" maxlength="1000">

            <input v-model.trim="password" type="password" name="password" placeholder="Password..." required
                minlength="8" class="form-control mb-4 shadow rounded-0" maxlength="2000">
            <input v-model.trim="passwordRepeat" type="password" name="password-repeat" placeholder="Repeat password..."
                class="form-control mb-4 shadow rounded-0">

            <div class="error-message">{{ error }}</div>

            <button type="submit" class="btn btn-primary">Register</button>
        </form>
    </div>
</template>