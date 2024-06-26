<script setup>
import { computed } from "vue";
import BackgroundEditor from "./BackgroundEditor.vue";
import ImageItem from "./ImageItem.vue";
import MiniImagesList from "@/components/custom-widgets/MiniImagesList.vue";
import ItemSelect from "@/components/custom-widgets/ItemSelect.vue";
import { imageToSrc, loadImage, objToImage } from "@/js/utilities/image-utility";
import { ref } from "vue";

const props = defineProps({
    isActive: {
        type: Boolean,
        required: true
    },
    currentScene: {
        type: Object,
        required: true
    },
    onClose: {
        type: Function,
        required: true
    },
    onUpdate: {
        type: Function,
        required: true
    },
    userImages: {
        type: Array,
        required: true
    },
    draws: {
        type: Object,
        required: true
    }
});

const images = computed(() => props.currentScene.images), length = computed(() => images.value.length);
const draws = computed(() => props.draws);

const createImage = (image, draw) => {
    const obj = {
        img: image,
        rect: {
            x: 5,
            y: 5,
            width: draw.width,
            height: draw.height,
            rotation: 0
        }
    };

    draws.value[image.name] = draw;

    return obj;
};

const updateImage = (index, image) => {
    if (index >= 0 && index < images.value.length) {
        images.value[index] = image;
        props.onUpdate(images.value);
    }
};

const addImage = (image, draw) => {
    images.value.push(createImage(image, draw));

    props.onUpdate(images.value);
    backgroundRef.value.redrawAll(images.value.length - 1);
};

const onImageSelect = (value) => {
    if (typeof value.img === "string") {
        objToImage(value, addImage);
        return;
    }

    addImage(value, value.img);
};

const onRemove = (index) => {
    if (index >= 0 && index < images.value.length) {
        images.value.splice(index, 1);
        props.onUpdate(images.value);
    }
    backgroundRef.value.redrawAll();
};

const backgroundRef = ref(null);
const selectedImage = ref(-1);

const onUp = (index) => {
    if (index >= 1 && index < images.value.length) {
        [images.value[index], images.value[index - 1]] = [images.value[index - 1], images.value[index]];
        props.onUpdate(images.value);
    }

    backgroundRef.value.redrawAll(index - 1);
};

const onDown = (index) => {
    if (index >= 0 && index < images.value.length - 1) {
        [images.value[index], images.value[index + 1]] = [images.value[index + 1], images.value[index]];

        props.onUpdate(images.value);
    }
    backgroundRef.value.redrawAll(index + 1);
};

const imageClick = (index) => {
    backgroundRef.value.redrawAll(index);
};
const onPress = (index) => {
    selectedImage.value = index;
};

</script>

<template>
    <div>
        <div v-if="isActive" class="img-editor-background">
            <div class="row my-row">
                <div class="col-12 col-md-9 canvas-container">
                    <i class="fa-regular fa-circle-xmark text-white custom-btn" @click="onClose"></i>
                    <BackgroundEditor ref="backgroundRef" :images="images" :images-length="length" :draws="draws"
                        :update-image="updateImage" :currentScene="currentScene" :on-press="onPress">
                    </BackgroundEditor>
                </div>
                <div class="col-12 col-md-3 images-part">
                    <div>
                        <label>{{ $t('choseFile') }}</label>
                        <ItemSelect :items="userImages" :on-selected="onImageSelect" item-key="img" content-key="name"
                            value-key="name" file-type="image/png" :load-function="loadImage">
                        </ItemSelect>
                    </div>

                    <div class="images">
                        <ImageItem v-for="(data, index) in images" :key="data.img.id" :src="imageToSrc(data.img)"
                            :on-remove="() => onRemove(index)" :on-up="() => onDown(index)" :on-down="() => onUp(index)"
                            :class="selectedImage === index ? 'border border-danger' : ''"
                            :on-see="() => imageClick(index)">
                        </ImageItem>

                    </div>

                </div>
            </div>

        </div>
        <MiniImagesList :images="images"></MiniImagesList>
    </div>
</template>
<style>
.file-input {
    width: 50%;
}

.canvas-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}

.images-part {
    background-color: white;
    height: 100%;

    padding-top: 2%;
    padding-bottom: 2%;
    display: flex;
    flex-direction: column;
}


.images {
    display: flex;
    flex-direction: column-reverse;
    align-items: center;

    overflow-y: auto;
}

.images div img {
    width: 90%;
    margin: 1%;
}

.my-row {
    width: 100%;
    height: 100vh;
}

.img-editor-background {
    z-index: 10000;
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;

    top: 0;
    left: 0;

    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.519);

    padding: 1%;
}
</style>
