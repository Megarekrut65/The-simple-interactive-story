<script setup>
import BigBanner from '@/components/BigBanner.vue';
import StoryCards from '@/components/StoryCards.vue';
import { searchPublishStories } from '@/js/firebase/story';
import { ref } from 'vue';

const props = defineProps({
    authorId: {
        type: String,
        required: false
    }
});

const perPage = ref(12);

const titleSearch = ref(""), authorSearch = ref(""), genreSearch = ref("");

const searchParams = ref({
    title: null,
    author: null,
    authorId: props.value,
    genre: null
});
const loadMore = (perPage, after) => {
    return searchPublishStories(searchParams.value.title, searchParams.value.author, searchParams.value.authorId, searchParams.value.genre, perPage, after);
};

loadMore(perPage.value);

const onSearch = () => {
    searchParams.value = {
        title: titleSearch.value,
        author: authorSearch.value,
        authorId: props.value,
        genre: genreSearch.value
    };

    loadMore(perPage.value);
};

const toLink = (data) => { return { name: 'story', params: { publishId: `${data.publish}` } } };
</script>
<template>
    <BigBanner :title="$t('title')" min-height="50vh"></BigBanner>

    <div class="basic-page">
        <section class="section">
            <div class="container">
                <div class="row">
                    <div class="col-lg-10 mx-auto text-center">
                        <h2>{{ $t('userStories') }}</h2>
                        <p>{{ $t('selectPopular') }}</p>
                        <div class="section-border"></div>
                    </div>
                </div>
                <div class="row" v-if="!authorId">
                    <div class="col-12 col-lg-3">
                        <input v-model="titleSearch" :placeholder="$t('storyTitle')" type="text">
                    </div>
                    <div class="col-12 col-lg-3">
                        <input v-model="authorSearch" :placeholder="$t('storyAuthor')" type="text">
                    </div>
                    <div class="col-12 col-lg-3">
                        <input v-model="genreSearch" :placeholder="$t('storyGenre')" type="text">
                    </div>
                    <div class="col-12 col-lg-3">
                        <input :value="$t('search')" @click="onSearch" type="button">
                    </div>
                </div>
            </div>
            <StoryCards :key="searchParams" :load-function="loadMore" :per-page="perPage" :to-link="toLink">
            </StoryCards>
        </section>
    </div>
</template>