<script setup>
import { onMounted, ref } from "vue";
import { loadImage } from "@/js/utilities/image-utility";
import BackgroundEditor from "./BackgroundEditor.vue";

const images = ref([]);

const successLoad = (res) => {
    images.value.push(res);
};
const rejectLoad = (err) => {
    console.log(err);
};

const updateImage = (index, image) => {
    if (index >= 0 && index < images.value.length) {
        image.value[index] = image;
    }
};

onMounted(() => {
    const fileInput = document.getElementById("fileInput");
    if (fileInput) fileInput.addEventListener("change", (event) => {
        loadImage(event, successLoad, rejectLoad);
    });

});

</script>

<template>
    <div class="img-editor-background">
        <div class="row my-row">
            <div class="col-12 col-md-9 col-xl-8 canvas-container" id="test">
                <BackgroundEditor :images="images" :update-image="updateImage"></BackgroundEditor>
            </div>
            <div class="col-12 col-md-3 col-xl-4 images-part">
                <div>
                    <label for="fileInput">{{ $t('choseFile') }}</label>
                    <input type="file" id="fileInput" accept=".png">
                </div>

                <div class="images">
                    <img v-for="(data, index) in images" :key="index" :src="data.src">
                </div>

            </div>
        </div>

    </div>
</template>
<style>
#fileInput {
    width: 50%;
}

.canvas-container {
    display: flex;
    justify-content: center;
    align-items: center;
}

.images-part {
    background-color: white;
    position: relative;
    height: 80vh;

    padding-top: 10px;
}

.images {
    display: flex;
    flex-direction: column;
    align-items: center;

    overflow-y: auto;

    height: 80%;
}

.images img {
    width: 90%;
    margin: 1%;
}

.my-row {
    width: 100%;
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
