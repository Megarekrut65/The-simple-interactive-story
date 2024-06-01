<script setup>
import { imageToSrc } from '@/js/utilities/image-utility';
import { computed, toRaw } from 'vue';


const props = defineProps({
    images: {
        type: Array,
        required: true
    }
});

const images = computed(() =>
    props.images.map(image => imageToSrc(toRaw(image)))
);
</script>
<template>
    <div class="row">
        <div class="col-12 col-md-6 col-xl-4 mt-4" v-for="data in images" :key="data">
            <img class="mini-image" :src="data">
        </div>

    </div>
</template>
<style>
.mini-image {
    width: 100%;
}
</style>