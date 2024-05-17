<script setup>
import MiniImagesList from '../custom-widgets/MiniImagesList.vue';
defineProps({
    image: {
        type: [Image, String, Object],
        required: false
    }
});
</script>
<template>
    <MiniImagesList :images="image ? [image] : []"></MiniImagesList>
</template>