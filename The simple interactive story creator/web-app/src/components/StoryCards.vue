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
    }
});

const stories = ref([]);
let after = null;
const isAll = ref(true);

const loadMore = () => {
    isAll.value = true;

    props.loadFunction(props.perPage, after).then(res => {
        stories.value = stories.value.concat(res.map(item => item.data()));
        if (res.length > 0) {
            after = res[res.length - 1]
            isAll.value = false;
        }
        if (res.length < props.perPage) {
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
            <StoryItem v-for="data in stories" :key="data.id" :data="data"></StoryItem>
        </div>
        <div class="row" v-if="!isAll">
            <div class="col-lg-4 col-md-6 mb-4 m-1">
                <button class="btn btn-success" @click="loadMore">{{ $t('loadMore') }}</button>
            </div>
        </div>
    </div>
</template>