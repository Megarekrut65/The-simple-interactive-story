<script setup>
import { imageToSrc } from '@/js/utilities/image-utility';
import { computed } from 'vue';


const props = defineProps({
    title: {
        type: String,
        required: false
    },
    minHeight: {
        type: String,
        required: false,
        default: "70vh"
    },
    imageHref: {
        type: [Object, String, Image],
        required: false
    }
});

const image = computed(() => props.imageHref ? imageToSrc(props.imageHref) :
    new URL("@/assets/images/banner/banner2.jpg", import.meta.url).href);
</script>

<template>
    <section class="banner bg-cover position-relative d-flex justify-content-center align-items-center"
        :style="{ backgroundImage: 'url(' + image + ')', minHeight: minHeight }">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="text-white font-weight-bold font-primary banner-title">{{ title }}</h1>
                </div>
            </div>
        </div>
    </section>
</template>
<style>
.banner-title {
    overflow-y: hidden;
    font-size: 50px;
}

@media (max-width: 575px) {
    .banner-title {
        font-size: 25px;
    }
}
</style>