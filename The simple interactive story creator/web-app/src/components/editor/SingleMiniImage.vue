<script setup>
import MiniImagesList from './image-editor/MiniImagesList.vue';

defineProps({
    image: {
        type: Object,
        required: false
    }
});
</script>
<template>
    <MiniImagesList :images="image !== undefined ? [{ img: image }] : []"></MiniImagesList>
</template>