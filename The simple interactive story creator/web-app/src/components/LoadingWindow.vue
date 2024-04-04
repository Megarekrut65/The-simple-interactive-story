<script setup>
import "../assets/css/loader.css"

defineProps({
    isLoading:{
        type:Boolean,
        required:false,
        default: false
    }
});
</script>
<template>
    <div class="loader" v-if="isLoading">
        <div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>
    </div>
</template>