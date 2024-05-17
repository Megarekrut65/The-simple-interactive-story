<script setup>
import { ref, watch } from 'vue';
import SingleMiniImage from './SingleMiniImage.vue';
import ItemSelect from './ItemSelect.vue';
import { loadImage } from '@/js/utilities/image-utility';

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
    <ItemSelect :items="images" :on-selected="onImageSelected" :initial="image?.name" item-key="img" content-key="name"
        value-key="name" file-type="image/png" :load-function="loadImage"></ItemSelect>
    <SingleMiniImage :image="image"></SingleMiniImage>
</template>