<script setup>
import { useRoute } from 'vue-router';
import { getPublish, getStory } from "@/js/firebase/story.js";
import { ref } from 'vue';
import LoadingWindow from "@/components/LoadingWindow.vue";
import BigBanner from '@/components/BigBanner.vue';
import InfoWindow from '@/components/InfoWindow.vue';
import i18n from '@/i18n';
import { imageToSrc } from '@/js/utilities/image-utility';

i18n.useT();

const { publishId } = useRoute().params;

const message = ref({
    active: false,
    title: "",
    message: ""
});

const isLoading = ref(true);
const publish = ref(null);
const story = ref(null);

getPublish(publishId).then((res) => {

    if (!res) {

        message.value = {
            active: true,
            title: i18n.t("error"),
            message: i18n.t("publishError")
        };
        return Promise.resolve();
    }
    publish.value = res;

    return getStory(res.authorId, res.storyId).then(storyRes => {
        story.value = storyRes;
        console.log(storyRes)

        return Promise.resolve();
    });

}).catch(err => {
    console.log(err);

    message.value = {
        active: true,
        title: i18n.t("error"),
        message: JSON.stringify(err)
    };
}).finally(() => {
    isLoading.value = false;
});



</script>

<template>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <InfoWindow :title="message.title" :message="message.message" v-model="message.active"></InfoWindow>
    <BigBanner :title="story?.title" :image-href="imageToSrc(story?.banner)"></BigBanner>
    <div class="basic-page">
        <section class="section">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <div v-if="publish && story" class="card">
                            <div class="card-body">
                                <div class="title-box">
                                    <h4 class="card-title">{{ story.title }}</h4>
                                    <RouterLink class="h4 card-text text-primary"
                                        :to="{ name: 'story-view', params: { publishId: story.publish } }"
                                        target="_blank">
                                        {{ $t('runStory') }}
                                    </RouterLink>
                                </div>
                                <p class="card-text"><strong>{{ $t('storyAuthor') }}</strong></p>
                                <p class="card-text">
                                    <RouterLink :to="{ name: 'author-stories', params: { authorId: story.authorId } }">
                                        {{ story.author }}
                                    </RouterLink>
                                </p>

                                <p class="card-text"><strong>{{ $t('storyGenre') }}</strong></p>
                                <p class="card-text">{{ story.genre }}</p>

                                <p class="card-text"><strong>{{ $t('storyDes') }}</strong></p>
                                <p class="card-text description-text">{{ story.description }}</p>

                                <p class="card-text"><strong>{{ $t('creatingDate') }}</strong></p>
                                <p class="card-text">{{
                                    story.creatingDate.toDate().toLocaleDateString() }}</p>

                                <p class="card-text"><strong>{{ $t('publishDate') }}</strong></p>
                                <p class="card-text">{{
                                    publish.publishDate.toDate().toLocaleDateString() }}</p>
                            </div>
                        </div>
                        <div v-else class="card">
                            <div class="card-body">
                                <h4 class="card-title">{{ $t('error') }}</h4>
                                <p class="card-text"><strong>{{ $t('publishError') }}</strong></p>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </div>

</template>
<style>
.description-text {
    white-space: pre-wrap;
    line-height: 0.8;
}

.title-box {
    display: flex;
    justify-content: space-between;
}
</style>