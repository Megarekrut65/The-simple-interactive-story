<script setup>
import BigBanner from '@/components/BigBanner.vue';
import DescriptionBanner from '@/components/home/DescriptionBanner.vue';
import StoryCards from '@/components/StoryCards.vue';
import { getPublishStories, getStory } from '@/js/firebase/story';

const loadMore = (perPage, after) => {
    return getPublishStories(perPage, after).then(publishRes => {
        const stories = publishRes.list.map(publish => getStory(publish.authorId, publish.storyId));
        return Promise.all(stories.map(res => res.catch(() => null))).then(res => {
            return { list: res.filter(story => story), last: publishRes.last };
        });
    });
};

const toLink = (data) => { return { name: 'story', params: { publishId: `${data.publish}` } } };
</script>
<template>
    <BigBanner :title="$t('title')" min-height="100vh"></BigBanner>

    <div class="basic-page">
        <DescriptionBanner></DescriptionBanner>
        <section class="section">
            <div class="container">
                <div class="row">
                    <div class="col-lg-10 mx-auto text-center">
                        <h2>{{ $t('popularStories') }}</h2>
                        <p>{{ $t('selectPopular') }}</p>
                        <div class="section-border"></div>
                    </div>
                </div>
            </div>
            <StoryCards :load-function="loadMore" :per-page="12" :to-link="toLink"></StoryCards>
        </section>
    </div>
</template>