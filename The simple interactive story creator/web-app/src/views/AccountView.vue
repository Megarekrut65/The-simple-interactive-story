<script setup>
import BigBanner from '@/components/BigBanner.vue';
import LocalizedLink from '@/components/l10n/LocalizedLink.vue';
import StoryItem from '@/components/StoryItem.vue';
import { getUser } from '@/js/firebase/auth';
import { getUserStories } from '@/js/firebase/story';
import { ref } from 'vue';

//const image = new URL("@/assets/images/stories/post-1.jpg", import.meta.url).href;

const stories = ref([]);
let after = null;
const perPage = 2;
const isAll = ref(true);

const loadMore = () => {
    isAll.value = true;
    const user = getUser();
    if (!user) return;

    getUserStories(user.uid, perPage, after).then(res => {
        stories.value = stories.value.concat(res.map(item => item.data()));
        if (res.length > 0) {
            after = res[res.length - 1]
            isAll.value = false;
        }
    });

};

loadMore();

</script>

<template>
    <BigBanner :title="$t('accountBanner')"></BigBanner>

    <section class="section">
        <div class="container">
            <div class="row">
                <div class="col-lg-10 mx-auto text-center">
                    <h2>{{ $t('yourStories') }}</h2>
                    <p>{{ $t('selectStory') }}</p>
                    <div class="section-border"></div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6 mb-4 add-container">
                    <LocalizedLink to="editor/new"><img src="@/assets/images/stories/plus.png" class="add-icon">
                    </LocalizedLink>
                </div>
                <StoryItem v-for="data in stories" :key="data.id" :data="data"></StoryItem>
            </div>
            <div class="row" v-if="!isAll">
                <div class="col-lg-4 col-md-6 mb-4 m-1">
                    <button class="btn btn-success" @click="loadMore">{{ $t('loadMore') }}</button>
                </div>
            </div>
        </div>
    </section>
</template>