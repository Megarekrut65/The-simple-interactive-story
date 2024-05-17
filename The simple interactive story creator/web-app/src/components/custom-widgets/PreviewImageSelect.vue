<script setup>
import { ref } from 'vue';
import SingleMiniImage from '../editor/SingleMiniImage.vue';
import ImageSelect from './ImageSelect.vue';

const props = defineProps({
    images: {
        type: Array,
        required: true
    },
    onSelected: {
        type: Function,
        required: true
    }

});


const image = ref(undefined);

const onImageSelected = (value) => {
    image.value = value;
    props.onSelected(value);
}

</script>

<template>
    <ImageSelect :images="images" :on-selected="onImageSelected"></ImageSelect>
    <SingleMiniImage :image="image"></SingleMiniImage>
</template>