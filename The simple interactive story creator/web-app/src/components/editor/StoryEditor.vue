<script setup>
import BigBanner from '../BigBanner.vue';
import { computed, ref, toRaw } from 'vue';
import { getUniqueName } from '@/js/utilities/text-utility';
import i18n from '@/i18n';
import { getUser, subscribeAuthChange } from '@/js/firebase/auth';
import { useRouter } from 'vue-router';
import SceneEditor from './SceneEditor.vue';
import { v4 } from 'uuid';
import { unityFonts } from '@/unity-assets/fonts/fonts';
import PreviewImageSelect from '../custom-widgets/PreviewImageSelect.vue';
import { getStory, getStoryScenes, getUserStorage, cascadeRemoveStory, setScene, setStory } from '@/js/firebase/story';
import LoadingWindow from '../LoadingWindow.vue';
import { uploadImage } from '@/js/storage-uploading';
import InfoWindow from '../InfoWindow.vue';
import PublishWindow from './PublishWindow.vue';
import { objToImage } from '@/js/utilities/image-utility';

const router = useRouter();

const props = defineProps({
    storyId: {
        type: String,
        required: false
    }
});

const storyLoaded = ref(true);

const isLoading = ref(true);
const message = ref({
    active: false,
    title: "",
    message: ""
});

const fonts = ref(unityFonts);

const userStorage = ref({
    images: [],
    sounds: []
});

const updateStorage = () => {
    const user = getUser();
    if (!user) return;

    return getUserStorage(user.uid).then(res => {
        if (res) userStorage.value = res;
    });
};

updateStorage();

const untitled = computed(() => i18n.t("untitled"));
const removeTitle = ref("");

const story = ref({
    id: v4(), title: untitled.value, banner: null, font: "Arial", description: "",
    author: "", publish: null, authorId: null, genre: ""
});
const scenes = ref({});
const currentSceneKey = ref(undefined);

subscribeAuthChange((user) => {
    if (props.storyId) return;

    if (user) {
        story.value.author = user.displayName;
        story.value.authorId = user.uid;
        return;
    }

    router.go();
});

const draws = ref({});

const loadStoryData = () => {
    isLoading.value = true;

    if (!props.storyId) return Promise.resolve();
    const user = getUser();
    if (!user) return Promise.reject("User not defined");

    return getStory(user.uid, props.storyId).then(res => {
        if (!res) return Promise.reject("There no story with given id");

        story.value = res;
        return getStoryScenes(user.uid, props.storyId).then(scenesRes => {
            scenesRes.forEach(scene => {
                scenes.value[scene.id] = scene;
                if (scene.isMain) currentSceneKey.value = scene.id;

                scene.images.forEach(item => {
                    objToImage(item.img, (_, draw) => {
                        draws.value[item.img.name] = draw;
                    });
                });
            });

            return Promise.resolve();
        });
    });
};

loadStoryData().catch(err => {
    console.log(err);
    storyLoaded.value = false;
    message.value = {
        active: true,
        title: i18n.t("error"),
        message: i18n.t("storyError"),
    }
}).finally(() => {
    isLoading.value = false;
});;

const makeMain = () => {
    Object.values(scenes.value).forEach(scene => {
        scene.isMain = scene.id === currentSceneKey.value;
    });
};

const createEmptyScene = (defaultTitle = null) => {
    const id = v4();
    const size = Object.keys(scenes.value).length;

    if (!defaultTitle) defaultTitle = untitled.value;

    const title = getUniqueName(Object.values(scenes.value).map(item => item.title), defaultTitle);
    const scene = {
        id: id, title: title, background: null, music: null, text: "",
        answers: [], images: [], isMain: size == 0
    };

    return scene;
};

const createSceneForAnswers = () => {
    const current = scenes.value[currentSceneKey.value];
    const title = current.title;

    const scene = createEmptyScene(title);
    scenes.value[scene.id] = scene;

    return scene;
};

const addScene = () => {
    const scene = createEmptyScene();
    scenes.value[scene.id] = scene;
    currentSceneKey.value = scene.id;
};

const removeCurrentScene = () => {
    const current = scenes.value[currentSceneKey.value];
    if (current.isMain || Object.keys(scenes.value).length <= 1) {
        message.value = {
            active: true,
            title: i18n.t("removeSceneTitle"),
            message: i18n.t("infoMainMessage"),
        }
        return;
    }

    for (let i in scenes.value) {
        for (let j in scenes.value[i].answers) {
            const answer = scenes.value[i].answers[j];
            if (answer.nextScene === current.id) {
                message.value = {
                    active: true,
                    title: i18n.t("removeSceneTitle"),
                    message: i18n.t("infoAnswerMessage") + " " + scenes.value[i].title,
                }
                return;
            }
        }
    }

    delete scenes.value[currentSceneKey.value];
    currentSceneKey.value = scenes.value[Object.keys(scenes.value)[0]].id;
}

const onBannerChanged = (value) => {
    story.value.banner = value;
};

const saveStory = () => {
    const user = getUser();
    if (!user) {
        return Promise.reject("No user");
    }

    isLoading.value = true;

    const value = toRaw(story.value);
    if (!value.creatingDate) value.creatingDate = new Date();

    return uploadImage(userStorage.value.images, value.banner).then(banner => {
        value.banner = banner;

        return setStory(user.uid, value).then(() => {
            if (props.storyId) return Promise.resolve();

            return setScene(user.uid, value.id, createEmptyScene()).then(() => {
                router.push({ name: "editor", params: { storyId: value.id } });
            });
        });
    }).catch(err => {
        console.log(err);
    }).finally(() => {
        isLoading.value = false;
    });
};

const submitStory = () => {
    saveStory();

    return false;
};

const removeStoryAction = () => {
    const user = getUser();
    if (!user || !props.storyId) return;
    isLoading.value = true;

    cascadeRemoveStory(user.uid, props.storyId, story.value.publish).then(() => {
        router.push({ name: "account" });
    })
        .catch(err => {
            console.log(err);
            isLoading.value = false;
        });
};

const publishActive = ref(false);
const storyForm = ref(null);

const onPublish = () => {
    if (!storyForm.value.checkValidity()) return;

    saveStory().then(() => {
        publishActive.value = true;
    });
};

</script>

<template>
    <PublishWindow v-model="publishActive" :story="story"></PublishWindow>
    <InfoWindow :title="message.title" :message="message.message" v-model="message.active"></InfoWindow>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <BigBanner min-height="50vh" :title="story.title" :image-href="story.banner"></BigBanner>

    <section class="section">
        <div class="container" v-if="storyLoaded">
            <div class="row">
                <div class="col">
                    <h3 class="font-tertiary mb-5">{{ $t("generalSettings") }}</h3>
                    <form onsubmit="return false;" action="#" @submit="submitStory" ref="storyForm">
                        <table class="form-table">
                            <tr>
                                <td><label class="star">{{ $t("storyTitle") }}</label></td>
                                <td><input v-model.trim="story.title" type="text" :placeholder="$t('storyHint')"
                                        required minlength="5" maxlength="50" style="width: 100%;"></td>
                            </tr>
                            <tr>
                                <td><label class="star">{{ $t("storyGenre") }}</label></td>
                                <td><input v-model.trim="story.genre" type="text" :placeholder="$t('genreHint')"
                                        required minlength="1" maxlength="50" style="width: 100%;"></td>
                            </tr>
                            <tr>
                                <td><label class="star">{{ $t('storyDes') }}</label></td>
                                <td>
                                    <textarea style="width: 100%;" :placeholder="$t('storyDesHint')"
                                        v-model="story.description" required maxlength="3000"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td><label for="preview">{{ $t("storyBanner") }}</label></td>
                                <td>
                                    <PreviewImageSelect :images="userStorage.images" :initial="story.banner"
                                        :on-selected="onBannerChanged">
                                    </PreviewImageSelect>
                                </td>
                            </tr>
                            <tr>
                                <td><label class="star">{{ $t("storyFont") }}</label></td>
                                <td>
                                    <div class="part-container">
                                        <div class="text-black font-label" :style="{ fontFamily: story.font }">{{
                                            $t('sample') }}
                                        </div>
                                        <select v-model="story.font" required>
                                            <option v-for="data in fonts" :key="data.id">
                                                {{ data.name }}
                                            </option>
                                        </select>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td><label class="star">{{ $t("storyAuthor") }}</label></td>
                                <td><input type="text" readonly required :value="story.author"></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>{{ $t("allFieldsMarked") }} <label class="star"></label></td>
                            </tr>
                            <tr>
                                <td><input type="submit" :value="$t('save')"></td>
                                <td><input v-if="storyId" type="button" class="btn-outline-primary disabled-btn"
                                        :value="$t('publish')" @click="onPublish" :disabled="!storyId"></td>
                            </tr>
                        </table>

                    </form>
                </div>
            </div>
            <div class="row mt-5" v-if="storyId">
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">{{ $t('scenes') }}</h3>
                    <div class="row">
                        <div class="col-12 col-md-6"><input type="button" class="btn-outline-custom"
                                :value="$t('addScene')" @click="addScene">
                        </div>
                        <div class="col-12 col-md-6"><input type="button" class="btn-outline-custom"
                                :value="$t('makeMain')" @click="makeMain">
                        </div>
                    </div>
                    <table>
                        <tr>
                            <td><label class="star">{{ $t('scene') }}</label></td>
                            <td>
                                <select required v-model="currentSceneKey">
                                    <option v-for="data in scenes" :key="data.id" :value="data.id">{{ data.title
                                        }}{{ data.isMain ? `(${$t('mainScene')})` : "" }}
                                    </option>
                                </select>

                            </td>
                        </tr>
                    </table>
                    <SceneEditor v-if="Object.keys(scenes).length > 0" :scenes="scenes"
                        :currentSceneKey="currentSceneKey" :user-storage="userStorage" :story-id="story.id"
                        :remove-scene="removeCurrentScene" :draws="draws" :create-scene="createSceneForAnswers">
                    </SceneEditor>
                </div>
            </div>
            <div class="row mt-5" v-if="storyId">
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">{{ $t('settings') }}</h3>
                    <div class="row">
                        <div class="col-12 border p-4">
                            <p lass="font-tertiary mb-1">{{ $t('removeTitle') }}</p>
                            <input type="text" v-model.trim="removeTitle">
                            <input type="button" class="btn-outline-danger" :value="$t('remove')"
                                @click="removeStoryAction" :disabled="removeTitle !== story.title">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<style>
.font-label {
    margin-top: 10px;
    vertical-align: middle;
}
</style>