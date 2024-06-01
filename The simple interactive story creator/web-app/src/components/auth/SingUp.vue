<script setup>
import { computed, ref } from 'vue'
import { createNewUser } from '@/js/firebase/auth';
import { useRouter } from 'vue-router';
import LoadingWindow from '../LoadingWindow.vue';
import { goNext } from '@/js/utilities/router-utility';
import i18n from '@/i18n';

const router = useRouter();

const isLoading = ref(false);

const nickname = ref(""), email = ref(""), password = ref(""), passwordRepeat = ref("");
const error = ref("");

const dontMatch = computed(() => i18n.t('dontMatch'));

const onSubmit = () => {
    if (password.value != passwordRepeat.value) {
        error.value = dontMatch.value;
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
        <h3>{{ $t('singUp') }}</h3>

        <form @submit="onSubmit" action="#" onsubmit="return false;">
            <input v-model.trim="nickname" type="text" name="name" :placeholder="$t('nicknameHint')" required
                minlength="2" class="form-control mb-4 shadow rounded-0" maxlength="1000">
            <input v-model.trim="email" type="email" name="email" :placeholder="$t('emailHint')" required minlength="4"
                class="form-control mb-4 shadow rounded-0" maxlength="1000">

            <input v-model.trim="password" type="password" name="password" :placeholder="$t('passwordHint')" required
                minlength="8" class="form-control mb-4 shadow rounded-0" maxlength="1000">
            <input v-model.trim="passwordRepeat" type="password" name="password-repeat" :placeholder="$t('repeatHint')"
                class="form-control mb-4 shadow rounded-0">

            <div class="error-message">{{ error }}</div>

            <button type="submit" class="btn btn-primary">{{ $t('register') }}</button>
        </form>
    </div>
</template>