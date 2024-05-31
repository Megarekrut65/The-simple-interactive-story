<script setup>
import { useRoute } from 'vue-router';
import PublishedStories from "@/components/PublishedStories.vue";
const { authorId } = useRoute().params;
</script>

<template>
    <PublishedStories :authorId="authorId"></PublishedStories>
</template>