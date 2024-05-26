<script setup>
import { imageToSrc } from '@/js/utilities/image-utility';
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

</script>

<template>
    <div class="col-lg-4 col-md-6 mb-4">
        <article class="card story-card">
            <img :src="image" alt="Story banner" class="card-img-top mb-2">
            <div class="card-body p-1 story-card-body">
                <time>{{ data.creatingDate.toDate().toLocaleDateString() }}</time>
                <p class="h4 card-title d-block my-3 text-dark">
                    {{ data.title }}
                </p>

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