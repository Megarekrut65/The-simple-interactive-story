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

const router = useRouter();

const props = defineProps({
    storyId: {
        type: String,
        required: false
    }
});

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
    id: v4(), title: untitled.value, banner: null, font: "Arial",
    author: "", private: true
});
const scenes = ref({});
const currentSceneKey = ref(undefined);

subscribeAuthChange((user) => {
    if (props.storyId) return;

    if (user) {
        story.value.author = user.displayName;
        return;
    }

    router.go();
});

const loadStoryData = () => {
    isLoading.value = true;

    if (!props.storyId) return Promise.reject();
    const user = getUser();
    if (!user) return Promise.reject();

    return getStory(user.uid, props.storyId).then(res => {
        if (!res) return Promise.reject();

        story.value = res;
        return getStoryScenes(user.uid, props.storyId).then(scenesRes => {
            scenesRes.forEach(scene => {
                scenes.value[scene.id] = scene;
                if (scene.isMain) currentSceneKey.value = scene.id;
            });

            return Promise.resolve();
        });
    });
};

loadStoryData().catch(err => {
    console.log(err);
}).finally(() => {
    isLoading.value = false;
});;

const makeMain = () => {
    Object.values(scenes.value).forEach(scene => {
        scene.isMain = scene.id === currentSceneKey.value;
    });
};

const createEmptyScene = () => {
    const id = v4();
    const size = Object.keys(scenes.value).length;

    const title = getUniqueName(Object.values(scenes.value).map(item => item.title), untitled.value);
    const scene = {
        id: id, title: title, background: null, music: null, text: "",
        answers: [], images: [], isMain: size == 0
    };

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

const submitStory = () => {
    const user = getUser();
    if (!user) {
        return false;
    }

    isLoading.value = true;

    const value = toRaw(story.value);
    if (!value.creatingDate) value.creatingDate = new Date();

    uploadImage(userStorage.value.images, value.banner).then(banner => {
        value.banner = banner;

        return setStory(user.uid, value).then(() => {
            if (props.storyId) return Promise.resolve();

            return setScene(user.uid, value.id, createEmptyScene()).then(() => {
                router.push({ name: "editor", params: { storyId: value.id } });
            });
        });
    })
        .catch(err => {
            console.log(err);
        }).finally(() => {
            isLoading.value = false;
        });

    return false;
};

const removeStoryAction = () => {
    const user = getUser();
    if (!user || !props.storyId) return;
    isLoading.value = true;

    cascadeRemoveStory(user.uid, props.storyId).then(() => {
        router.push({ name: "account" });
    })
        .catch(err => {
            console.log(err);
            isLoading.value = false;
        });
};


</script>

<template>
    <InfoWindow :title="message.title" :message="message.message" v-model="message.active"></InfoWindow>
    <LoadingWindow :is-loading="isLoading"></LoadingWindow>
    <BigBanner min-height="50vh" :title="story.title" :image-href="story.banner"></BigBanner>

    <section class="section">
        <div class="container">
            <div class="row">
                <div class="col">
                    <h3 class="font-tertiary mb-5">{{ $t("generalSettings") }}</h3>
                    <form onsubmit="return false;" action="#" @submit="submitStory">
                        <table class="form-table">
                            <tr>
                                <td><label class="star" for="title">{{ $t("storyTitle") }}</label></td>
                                <td><input v-model.trim="story.title" name="title" type="text"
                                        placeholder="The simple story" required minlength="5" maxlength="50"
                                        style="width: 100%;"></td>
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
                                <td><label for="private" :title="$t('storyPrivateDes')">{{ $t("storyPrivate") }}</label>
                                </td>
                                <td><input type="checkbox" checked v-model="story.private"></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>{{ $t("allFieldsMarked") }} <label class="star"></label></td>
                            </tr>
                            <tr>
                                <td><input type="submit" :value="$t('save')" style="margin-top: 20px;"></td>
                                <td></td>
                            </tr>
                        </table>

                    </form>
                </div>
            </div>
            <div class="row mt-5" v-if="storyId">
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">{{ $t('scenes') }}</h3>
                    <div class="row">
                        <div class="col-12 col-md-4"><input type="button" :value="$t('addScene')" @click="addScene">
                        </div>
                        <div class="col-12 col-md-4"><input type="button" :value="$t('viewScene')">
                        </div>
                        <div class="col-12 col-md-4"><input type="button" :value="$t('makeMain')" @click="makeMain">
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
                        :remove-scene="removeCurrentScene">
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
                            <input type="button" :value="$t('remove')" @click="removeStoryAction"
                                :disabled="removeTitle !== story.title">
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