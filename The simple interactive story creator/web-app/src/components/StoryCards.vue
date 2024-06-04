<script setup>
import LocalizedLink from '@/components/l10n/LocalizedLink.vue';
import StoryItem from '@/components/StoryItem.vue';
import { ref } from 'vue';

const props = defineProps({
    loadFunction: {
        type: Function,
        required: true
    },
    perPage: {
        type: Number,
        required: false,
        default: 12
    },
    addNew: {
        type: Boolean,
        required: false,
        default: false
    },
    toLink: {
        type: Function,
        required: false
    }
});

const stories = ref([]);
let after = null;
const isAll = ref(true);

const loadMore = () => {
    isAll.value = true;

    props.loadFunction(props.perPage, after).then(res => {
        stories.value = stories.value.concat(res.list);
        if (res.list.length > 0) {
            after = res.last;
            isAll.value = false;
        }
        if (res.list.length < props.perPage) {
            isAll.value = true;
        }
    });
};

loadMore();
</script>
<template>
    <div class="container">
        <div class="row">
            <div v-if="addNew" class="col-lg-4 col-md-6 mb-4 add-container">
                <LocalizedLink to="editor/new"><img src="@/assets/images/stories/plus.png" class="add-icon">
                </LocalizedLink>
            </div>
            <StoryItem v-for="data in stories" :key="data.id" :data="data" :to-link="toLink"></StoryItem>
        </div>
        <div class="row" v-if="!isAll">
            <div class="col-lg-4 col-md-6 mb-4 m-1">
                <button class="btn btn-success" @click="loadMore">{{ $t('loadMore') }}</button>
            </div>
        </div>
        <div class="row" v-if="stories.length == 0">
            <div class="col-12 text-center">
                <h5>{{ $t('empty') }}</h5>
            </div>
        </div>
    </div>
</template>