<script setup>
import { ref } from 'vue';
import SafeDatalist from './SafeDatalist.vue';
import { loadImage } from '@/js/utilities/image-utility';

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

const selectImage = ref("");

const image = ref(undefined);

const successLoad = (res) => {
    selectImage.value = "";
    image.value = { id: res.id, img: res, name: res.name };

    props.onSelected(image.value);
};
const rejectLoad = (err) => {
    console.log(err);
    image.value = undefined;
    props.onSelected(undefined);
};
const onInputChanged = (event) => {
    loadImage(event, successLoad, rejectLoad)
};

const onImageSelected = (value) => {
    image.value = value;

    props.onSelected(image.value);
};
</script>

<template>
    <div class="part-container">
        <SafeDatalist v-model="selectImage" :list="images" content-key="name" value-key="name"
            :on-select="onImageSelected">
        </SafeDatalist>
        <input type="file" accept="image/png" @change="onInputChanged">
    </div>
</template>