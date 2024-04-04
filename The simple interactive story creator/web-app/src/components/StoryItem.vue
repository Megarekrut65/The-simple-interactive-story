<script setup>
import { RouterLink } from 'vue-router'

defineProps({
    id:{
        type:String,
        required: true
    },
    title:{
        type:String,
        required: true
    },
    date:{
        type:String,
        required: true
    },
    image:{
        type:String,
        required: true
    }
});

</script>

<template>
    <div class="col-lg-4 col-md-6 mb-4 m-1">
        <article class="card">
            <img :src="image" alt="Story icon" class="card-img-top mb-2">
            <div class="card-body p-1">
                <time>{{date}}</time>
                <RouterLink :to="{ name: 'editor', params: { storyId: `${id}` } }" class="h4 card-title d-block my-3 text-dark hover-text-underline">
                    {{ title }}
                </RouterLink>
                <RouterLink class="btn btn-transparent" :to="{ name: 'editor', params: { storyId: `${id}` } }">View</RouterLink>
            </div>
        </article>
    </div>
</template>