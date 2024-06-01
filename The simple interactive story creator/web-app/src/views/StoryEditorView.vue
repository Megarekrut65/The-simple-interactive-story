<script setup>
import { useRoute } from 'vue-router';
import StoryEditor from "@/components/editor/StoryEditor.vue";

const {storyId} = useRoute().params;

</script>
<template>
    <StoryEditor :story-id="storyId"></StoryEditor>
</template>