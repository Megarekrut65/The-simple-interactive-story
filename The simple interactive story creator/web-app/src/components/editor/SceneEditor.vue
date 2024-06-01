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
import QuestionWindow from '../QuestionWindow.vue';
import i18n from '@/i18n';
import { uploadImage, uploadSound } from '@/js/storage-uploading';
import { setScene } from '@/js/firebase/story';

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
    },
    removeScene: {
        type: Function,
        required: true
    },
    draws: {
        type: Object,
        required: true
    },
    createScene: {
        type: Function,
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

const onSceneSave = async () => {
    saveScene(toRaw(currentScene.value));
    return false;
};

const saveScene = (scene) => {
    const user = getUser();
    if (!user) return false;

    isLoading.value = true;

    const uploadBackground = uploadImage(props.userStorage.images, scene.background).then(background => {
        scene.background = background;
        return Promise.resolve();
    });
    const uploadMusic = uploadSound(props.userStorage.sounds, scene.music).then(music => {
        scene.music = music;
        return Promise.resolve();
    });

    return Promise.all([uploadBackground, uploadMusic]).then(() => {

        const allImages = scene.images.map((image) =>
            uploadImage(props.userStorage.images, image.img).then(res => {
                image.img = res;

                return Promise.resolve();
            })
        );

        return Promise.all(allImages);
    }).then(() => {
        return setScene(user.uid, props.storyId, scene);
    }).catch(err => {
        console.log(err);
        return Promise.reject();
    }).finally(() => {
        isLoading.value = false
        return Promise.resolve();
    });
};


const saveAll = async () => {
    for (let i in scenes.value) {
        await saveScene(toRaw(scenes.value[i]))
    }
};

const question = ref({
    title: "",
    question: "",
    active: false,
    yes: () => { }
});

const removeCurrentScene = () => {
    question.value = {
        title: i18n.t("removeSceneTitle"),
        question: i18n.t("removeSceneQuestion"),
        active: true,
        yes: props.removeScene
    };
};

</script>

<template>
    <QuestionWindow v-model="question.active" :title="question.title" :question="question.question"
        :yes-function="question.yes"></QuestionWindow>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <form @submit="onSceneSave" onsubmit="return false;" action="#">
        <table class="form-table">
            <tr>
                <td><label>{{ $t('sceneTitle') }}</label></td>
                <td><input type="text" :placeholder="$t('sceneHint')" v-model="currentScene.title" maxlength="50"
                        @change="onTitleChanged"></td>
            </tr>

            <tr>
                <td><label>{{ $t('sceneText') }}</label></td>
                <td>
                    <textarea style="width: 100%;" :placeholder="$t('sceneTextHint')" v-model="currentScene.text"
                        maxlength="3000"></textarea>
                </td>
            </tr>

            <tr>
                <td><label>{{ $t('sceneBackground') }}</label></td>
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
                <td><label class="td-icon">{{ $t('sceneAnswers') }}<i class="fa-regular fa-square-plus custom-btn"
                            @click="onAddAnswer"></i></label></td>
                <td>
                    <AnswersEditor :answers="currentScene.answers" :scenes="scenes" :on-update="onUpdateAnswers"
                        :create-scene="createScene">
                    </AnswersEditor>
                </td>
            </tr>

            <tr>
                <td><label class="td-icon">{{ $t('sceneImages') }}<i class="fa-solid fa-pencil custom-btn"
                            @click="editorIsActive = !editorIsActive"></i></label>
                </td>
                <td>
                    <ImagesEditor :is-active="editorIsActive" :currentScene="currentScene" :on-close="onSceneClose"
                        :draws="draws" :on-update="onUpdateImages" :user-images="userStorage.images">
                    </ImagesEditor>
                </td>
            </tr>

            <tr>
                <td></td>
                <td class="row text-center mt-4">
                    <div class="col-12 col-sm-4">
                        <input type="submit" :value="$t('save')">
                    </div>
                    <div class="col-12 col-sm-4">
                        <input type="button" :value="$t('saveAll')" @click="saveAll" class="btn-outline-success">
                    </div>
                    <div class="col-12 col-sm-4">
                        <input type="button" :value="$t('remove')" @click="removeCurrentScene"
                            class="btn-outline-danger">
                    </div>
                </td>
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