<script setup>
import { useRoute } from 'vue-router';
import { getPublish } from "@/js/firebase/story.js";
import { ref } from 'vue';
import LoadingWindow from "@/components/LoadingWindow.vue";
import BigBanner from '@/components/BigBanner.vue';
import InfoWindow from '@/components/InfoWindow.vue';
import i18n from '@/i18n';

i18n.useT();

const { publishId } = useRoute().params;

const message = ref({
    active: false,
    title: "",
    message: ""
});

const isLoading = ref(true);
const publish = ref(null);

getPublish(publishId).then((res) => {

    if (res) {
        publish.value = res;
        return;
    }

    message.value = {
        active: true,
        title: i18n.t("error"),
        message: i18n.t("publishError")
    };
}).catch(err => {
    console.log(err);
}).finally(() => {
    isLoading.value = false;
});

</script>

<template>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <InfoWindow :title="message.title" :message="message.message" v-model="message.active"></InfoWindow>
    <BigBanner title="Story"></BigBanner>
    <div class="basic-page">
        <div v-if="publish">
            <div>{{ publish.id }}</div>
            <div>{{ publish.storyId }}</div>
            <div>{{ publish.authorId }}</div>
            <div>{{ publish.publishDate.toDate().toLocaleDateString() }}</div>
        </div>
    </div>

</template>