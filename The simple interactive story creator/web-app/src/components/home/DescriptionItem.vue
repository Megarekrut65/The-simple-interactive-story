<script setup>
defineProps({
    title: {
        type: String,
        required: true
    },
    description: {
        type: String,
        required: true
    }
});

</script>

<template>
    <div class="media">
        <i class="icon text-gradient-primary ti-vector mr-3"></i>
        <div class="media-body">
            <h4 class="text-white">{{ title }}</h4>
            <p class="text-light">{{ description }}</p>
        </div>
    </div>
</template>