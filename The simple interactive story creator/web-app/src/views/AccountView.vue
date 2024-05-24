<script setup>
import BigBanner from '@/components/BigBanner.vue';
import StoryCards from '@/components/StoryCards.vue';
import { getUser, subscribeAuthChange } from '@/js/firebase/auth';
import { getUserStories } from '@/js/firebase/story';
import { useRouter } from 'vue-router';

const router = useRouter();

subscribeAuthChange((user) => {
    if (!user) {
        router.go();
    }
});

const loadMore = (perPage, after) => {
    const user = getUser();
    if (!user) return;

    return getUserStories(user.uid, perPage, after);
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
        </div>
        <StoryCards :load-function="loadMore" :per-page="5" :add-new="true"></StoryCards>
    </section>
</template>