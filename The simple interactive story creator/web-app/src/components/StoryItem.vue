<script setup>
import { getUser } from '@/js/firebase/auth';
import { imageToSrc } from '@/js/utilities/image-utility';
import { computed } from 'vue';
import { RouterLink } from 'vue-router'

const props = defineProps({
    data: {
        type: Object,
        required: true
    },
    toLink: {
        type: Function,
        required: false
    }
});


const image = props.data.banner ? imageToSrc(props.data.banner) :
    new URL("@/assets/images/banner/banner2.jpg", import.meta.url).href;

const isAuthor = computed(() => {
    const user = getUser();
    if (!user) return false;

    return user.uid === props.data.authorId;
});

</script>

<template>
    <div class="col-lg-4 col-md-6 mb-4">
        <article class="card story-card">
            <img :src="image" alt="Story banner" class="card-img-top mb-2">
            <div class="card-body p-1 mb-0 story-card-body">
                <time>{{ data.creatingDate.toDate().toLocaleDateString() }}</time>
                <p class="h4 card-title d-block mt-3 text-dark">
                    {{ data.title }}
                </p>
                <p :class="isAuthor ? 'text-danger' : 'text-secondary'">{{ isAuthor ? $t('you') : data.author }}</p>

            </div>
            <RouterLink v-if="toLink" class="btn btn-transparent text-left pl-2" :to="toLink(data)">
                {{ $t('view') }}
            </RouterLink>
        </article>
    </div>
</template>
<style>
.story-card {
    height: 100%;
}

.card-img-top {
    object-fit: cover;
    height: 200px;
}
</style>