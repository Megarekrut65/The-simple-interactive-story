<script setup>
import { loadImage } from '@/js/utilities/image-utility';
import ImagesEditor from './image-editor/ImagesEditor.vue';
import { computed, ref } from 'vue';
import SingleMiniImage from './SingleMiniImage.vue';
import AnswersEditor from './AnswersEditor.vue';
import { v4 } from 'uuid';
import SafeDatalist from '../custom-widgets/SafeDatalist.vue';

const props = defineProps({
    scenes: {
        type: Object,
        required: true
    },
    currentSceneKey: {
        type: String,
        required: true
    },
    userStorage: {
        type: Object,
        required: true
    }
});

const isActive = ref(false);

const scenes = computed(() => props.scenes);
const currentKey = computed(() => props.currentSceneKey);
const currentScene = computed(() => props.scenes[currentKey.value]);

const backgroundSuccessLoad = (res) => {
    currentScene.value.background = res;
};
const rejectLoad = (err) => {
    console.log(err);
};
const onBackgroundInputChanged = (event) => {
    loadImage(event, backgroundSuccessLoad, rejectLoad);
};

const onBackgroundChanged = (value) => {
    backgroundSuccessLoad(value);
};

const onMusicInputChanged = (value) => {
    console.log(value)
};
const onMusicChanged = (value) => {
    console.log(value)
};

const onSceneClose = () => {
    isActive.value = false;
};

const onUpdateImages = (images) => {
    currentScene.value.images = images;
};
const onUpdateAnswers = (answers) => {
    currentScene.value.answers = answers;
};
const onAddAnswer = () => {
    currentScene.value.answers.push({ text: "", nextScene: "", id: v4() });
};

const onSceneSave = () => {
    console.log(currentScene.value);
};
</script>

<template>
    <form onsubmit="return false;" action="#">
        <table class="form-table">
            <tr>
                <td><label class="star">{{ $t('sceneTitle') }}</label></td>
                <td><input type="text" :placeholder="$t('sceneHint')" v-model="currentScene.title" minlength="5"
                        required></td>
            </tr>

            <tr>
                <td><label class="star" for="background">{{ $t('sceneBackground') }}</label></td>
                <td class="form-right">
                    <div class="part-container">
                        <SafeDatalist :list="userStorage.images" content-key="name" :on-select="onBackgroundChanged">
                        </SafeDatalist>
                        <input type="file" accept="image/png" @change="onBackgroundInputChanged">
                    </div>
                    <SingleMiniImage :image="currentScene.background"></SingleMiniImage>
                </td>
            </tr>

            <tr>
                <td><label>{{ $t('sceneBackgroundMusic') }}</label></td>
                <td>
                    <div class="part-container">
                        <SafeDatalist :list="userStorage.sounds" content-key="name" :on-select="onMusicChanged">
                        </SafeDatalist>
                        <input type="file" accept="audio/mp3" @change="onMusicInputChanged">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label class="star" for="text">{{ $t('sceneText') }}</label></td>
                <td>
                    <textarea style="width: 100%;" id="text" :placeholder="$t('sceneTextHint')" name="text"
                        required></textarea>
                </td>
            </tr>

            <tr>
                <td><label class="td-icon">{{ $t('sceneAnswers') }}<i class="fa-regular fa-square-plus custom-btn"
                            @click="onAddAnswer"></i></label></td>
                <td>
                    <AnswersEditor :answers="currentScene.answers" :scenes="scenes" :on-update="onUpdateAnswers">
                    </AnswersEditor>
                </td>
            </tr>

            <tr>
                <td><label class="td-icon">{{ $t('sceneImages') }}<i class="fa-solid fa-pencil custom-btn"
                            @click="isActive = !isActive"></i></label>
                </td>
                <td>
                    <ImagesEditor :is-active="isActive" :currentScene="currentScene" :on-close="onSceneClose"
                        :on-update="onUpdateImages">
                    </ImagesEditor>
                </td>
            </tr>

            <tr>
                <td></td>
                <td>{{ $t("allFieldsMarked") }} <label class="star"></label></td>
            </tr>
            <tr>
                <td><input type="submit" :value="$t('save')" style="margin-top: 20px;" @click="onSceneSave"></td>
                <td><input type="button" :value="$t('remove')" style="margin-top: 20px;"></td>
            </tr>
        </table>

    </form>
</template>
<style>
.form-right {
    background-color: rgba(0, 0, 0, 0.033);
    padding: 10px;
}

.td-icon {
    display: flex;
    align-items: center;
}

.td-icon>i {
    margin-left: 5px;
}
</style>