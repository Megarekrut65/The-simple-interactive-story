<script setup>
import ImagesEditor from './image-editor/ImagesEditor.vue';
import { computed, ref, toRaw } from 'vue';
import AnswersEditor from './AnswersEditor.vue';
import { v4 } from 'uuid';
import PreviewImageSelect from '../custom-widgets/PreviewImageSelect.vue';
import PreviewSoundSelect from '../custom-widgets/PreviewSoundSelect.vue';
import { getUniqueName } from '@/js/utilities/text-utility';
//import { setScene } from '@/js/firebase/story';
import { getUser } from '@/js/firebase/auth';
import LoadingWindow from '../LoadingWindow.vue';

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
    },
    storyId: {
        type: String,
        required: true
    }
});

const editorIsActive = ref(false);
const isLoading = ref(false);

const scenes = computed(() => props.scenes);
const currentScene = computed(() => props.scenes[props.currentSceneKey]);

const onTitleChanged = () => {
    const scene = currentScene.value;

    for (let i in scenes.value) {
        if (scenes.value[i].title === scene.title && scenes.value[i].id !== scene.id) {
            scene.title = getUniqueName(Object.values(scenes.value).map(item => item.title), scene.title);
        }
    }
};

const onBackgroundSelect = (value) => {
    currentScene.value.background = value;
};

const onMusicSelected = (value) => {
    currentScene.value.music = value;
};

const onSceneClose = () => {
    editorIsActive.value = false;
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
    const user = getUser();
    if (!user) return false;

    isLoading.value = true;

    const scene = toRaw(currentScene.value);
    console.log(scene);

    return false;
    /*setScene(user.uid, props.storyId, scene).then(() => {
        isLoading.value = false;
    });*/

};
</script>

<template>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <form @submit="onSceneSave" onsubmit="return false;" action="#">
        <table class="form-table">
            <tr>
                <td><label class="star">{{ $t('sceneTitle') }}</label></td>
                <td><input type="text" :placeholder="$t('sceneHint')" v-model="currentScene.title" minlength="5"
                        required @change="onTitleChanged"></td>
            </tr>

            <tr>
                <td><label class="star" for="background">{{ $t('sceneBackground') }}</label></td>
                <td class="form-right">
                    <PreviewImageSelect :images="userStorage.images" :initial="currentScene.background"
                        :on-selected="onBackgroundSelect">
                    </PreviewImageSelect>
                </td>
            </tr>

            <tr>
                <td><label>{{ $t('sceneBackgroundMusic') }}</label></td>
                <td>
                    <br>
                    <PreviewSoundSelect :sounds="userStorage.sounds" :initial="currentScene.music"
                        :on-selected="onMusicSelected"></PreviewSoundSelect>
                </td>
            </tr>

            <tr>
                <td><label class="star">{{ $t('sceneText') }}</label></td>
                <td>
                    <textarea style="width: 100%;" :placeholder="$t('sceneTextHint')" required
                        v-model="currentScene.text"></textarea>
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
                            @click="editorIsActive = !editorIsActive"></i></label>
                </td>
                <td>
                    <ImagesEditor :is-active="editorIsActive" :currentScene="currentScene" :on-close="onSceneClose"
                        :on-update="onUpdateImages" :user-images="userStorage.images">
                    </ImagesEditor>
                </td>
            </tr>

            <tr>
                <td></td>
                <td>{{ $t("allFieldsMarked") }} <label class="star"></label></td>
            </tr>
            <tr>
                <td><input type="submit" :value="$t('save')" style="margin-top: 20px;"></td>
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