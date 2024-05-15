<script setup>
defineProps({
    src: {
        type: String,
        required: true
    },
    onRemove: {
        type: Function,
        required: true
    }
})
</script>

<template>
    <div class="image-container">
        <img :src="src">
        <div class="panel">
            <i class="fa-solid fa-trash-can text-danger custom-btn" @click="onRemove" :title="$t('removeImage')"></i>
        </div>
    </div>
</template>

<style>
.image-container {
    box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
    margin: 5px;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.panel {
    display: flex;
    padding: 5px;
    justify-content: space-between;
}

.panel i {
    margin-left: 5px;
}
</style>