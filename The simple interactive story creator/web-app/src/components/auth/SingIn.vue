<script setup>
import { ref } from 'vue'
import { loginUserEmail } from '@/js/firebase/auth';
import LoadingWindow from '../LoadingWindow.vue';
import { goNext } from '@/js/utilities/router-utility';
import { useRouter } from 'vue-router';

const router = useRouter();
const isLoading = ref(false);

const email = ref(""), password = ref("");
const error = ref("");

const onSubmit = () => {
    error.value = "";
    isLoading.value = true;

    loginUserEmail(email.value, password.value).then(() => goNext(router))
        .catch(err => {
            console.log(err)
            isLoading.value = false;
            error.value = err;
        });
};

</script>

<template>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <div class="border border-dark p-3 m-lg-3 m-1">
        <h3>Sing In</h3>

        <form action="#" @submit="onSubmit" onsubmit="return false;">
            <input v-model="email" type="email" name="email" autocomplete="email" placeholder="Email..."
                class="form-control mb-4 shadow rounded-0" required>
            <input v-model="password" type="password" name="password" placeholder="Password..."
                class="form-control mb-4 shadow rounded-0" required>

            <div class="error-message">{{ error }}</div>

            <p>Forgot Password?</p>

            <button type="submit" class="btn btn-primary">Login</button>
        </form>
    </div>
</template>