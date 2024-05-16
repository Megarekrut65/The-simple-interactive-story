<script setup>

defineProps({
    images: {
        type: Array,
        required: true
    }
});
</script>
<template>
    <div class="row">
        <div class="col-12 col-md-6 col-xl-4 mt-4" v-for="data in images" :key="data.id">
            <img class="mini-image" :src="data.img.src">
        </div>

    </div>
</template>
<style>
.mini-image {
    width: 100%;
}
</style>