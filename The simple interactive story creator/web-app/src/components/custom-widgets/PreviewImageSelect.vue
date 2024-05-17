<script setup>
import { ref, watch } from 'vue';
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
    },
    initial: {
        required: false,
        default: undefined
    }
});


const image = ref(props.initial);

watch(() => props.initial, () => {
    image.value = props.initial;
});

const onImageSelected = (value) => {
    image.value = value;
    props.onSelected(value);
}

</script>

<template>
    <ImageSelect :images="images" :on-selected="onImageSelected" :initial="image?.name"></ImageSelect>
    <SingleMiniImage :image="image"></SingleMiniImage>
</template>