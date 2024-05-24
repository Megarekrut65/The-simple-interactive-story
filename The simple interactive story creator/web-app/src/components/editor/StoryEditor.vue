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
import { uploadFileAndGetUrl } from '@/js/firebase/storage';
import { getStory, getStoryScenes, getUserStorage, cascadeRemoveStory, setScene, setStorageImages, setStory } from '@/js/firebase/story';
import { imageToSrc } from '@/js/utilities/image-utility';

const router = useRouter();

const props = defineProps({
    storyId: {
        type: String,
        required: false
    }
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

    if (!props.storyId) return;
    const user = getUser();
    if (!user) return;

    getStory(user.uid, props.storyId).then(res => {
        if (!res) return;

        story.value = res;
        getStoryScenes(user.uid, props.storyId).then(scenesRes => {
            scenesRes.forEach(scene => {
                scenes.value[scene.id] = scene;
                if (scene.isMain) currentSceneKey.value = scene.id;
            });
        })
    });
};

loadStoryData();

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

const onBannerChanged = (value) => {
    story.value.banner = value;
};

const uploadBanner = (user, value, resolve) => {
    if (!value.banner) {
        resolve();
        return;
    }

    if (!value.banner.file) {
        value.banner = imageToSrc(value.banner);
        resolve();
        return;
    }

    uploadFileAndGetUrl(user.uid, "images", value.banner.file).then((res) => {
        const image = { id: value.banner.id, name: value.banner.name, img: res };
        userStorage.value.images.push(image);
        value.banner = res;

        return setStorageImages(user.uid, userStorage.value.images).then(resolve);
    });
};

const submitStory = () => {
    const user = getUser();
    if (!user) {
        return false;
    }

    const value = toRaw(story.value);
    if (!value.creatingDate) value.creatingDate = new Date();

    const promise = new Promise((resolve) => uploadBanner(user, value, resolve));
    promise.then(() => {
        return setStory(user.uid, value).then(() => {
            if (props.storyId) return;

            return setScene(user.uid, value.id, createEmptyScene()).then(() => {
                router.push({ name: "editor", params: { storyId: value.id } });
            });
        });
    })
        .catch(err => {
            console.log(err);
        });

    return false;
};

const removeStoryAction = () => {
    const user = getUser();
    if (!user || !props.storyId) return;

    cascadeRemoveStory(user.uid, props.storyId).then(() => {
        router.push({ name: "account" });
    });
};


</script>

<template>
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
                                <td><label class="star" for="font">{{ $t("storyFont") }}</label></td>
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
                        :currentSceneKey="currentSceneKey" :user-storage="userStorage" :story-id="story.id">
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