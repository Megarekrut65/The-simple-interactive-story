<script setup>
import BigBanner from '../BigBanner.vue';
import { computed, ref } from 'vue';
import { getUniqueName, textToId } from '@/js/utilities/text-utility';
import i18n from '@/i18n';
import { subscribeAuthChange } from '@/js/firebase/auth';
import { useRouter } from 'vue-router';
import SceneEditor from './SceneEditor.vue';
import { v4 } from 'uuid';
import { unityFonts } from '@/unity-assets/fonts/fonts';
import PreviewImageSelect from '../custom-widgets/PreviewImageSelect.vue';

const router = useRouter();

const props = defineProps({
    storyId: {
        type: String,
        required: false
    }
});

const fonts = ref(unityFonts);


const userStorage = ref({
    images: [{ id: "dwdwd", img: "https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg", name: "image" }],
    sounds: [{ id: "wdwdw", sound: new Audio(), name: "sww.mp3" }]
});

const untitled = computed(() => i18n.t("untitled"));

const story = ref({
    id: props.storyId ? props.storyId : "", title: untitled, banner: undefined, font: "Arial",
    author: "", private: true
});
const scenes = ref({});
const currentSceneKey = ref(undefined);

const addScene = () => {
    const id = v4();
    const size = Object.keys(scenes.value).length;

    const title = getUniqueName(Object.values(scenes.value).map(item => item.title), untitled.value);
    scenes.value[id] = {
        id: id, title: title, background: undefined, music: undefined, text: "",
        answers: [], images: [], isMain: size == 0
    };
    currentSceneKey.value = id;
};

if (Object.keys(scenes.value).length == 0) addScene();

const makeMain = () => {
    Object.values(scenes.value).forEach(scene => {
        scene.isMain = scene.id === currentSceneKey.value;
    });
};

const updateId = () => {
    if (story.value.title === "") {
        story.value.id = "";
        return;
    }

    story.value.id = textToId(story.value.title);
};
updateId();

subscribeAuthChange((user) => {
    if (user) {
        story.value.author = user.displayName;
        return;
    }

    router.go();
});


const onBannerChanged = (value) => {
    story.value.banner = value;
};

const submitStory = () => {
    console.log(story.value);
    return false;
};




</script>

<template>
    <BigBanner min-height="50vh" :title="story.title"></BigBanner>

    <section class="section">
        <div class="container">
            <div class="row">
                <div class="col">
                    <h3 class="font-tertiary mb-5">{{ $t("generalSettings") }}</h3>
                    <form onsubmit="return false;" action="#" @submit="submitStory">
                        <table class="form-table">
                            <tr>
                                <td><label class="star" for="identifier">{{ $t("storyId") }}</label></td>
                                <td><input v-model="story.id" name="identifier" type="text" required readonly
                                        :placeholder="$t('generatedAuto')" style="width: 100%;"></td>
                            </tr>

                            <tr>
                                <td><label class="star" for="title">{{ $t("storyTitle") }}</label></td>
                                <td><input v-model="story.title" @input="updateId" name="title" type="text"
                                        placeholder="The simple story" required minlength="5" style="width: 100%;"></td>
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
                                        <div class="text-black" :style="{ fontFamily: story.font }">{{ $t('sample') }}
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
                                <td><label class="star" for="author">{{ $t("storyAuthor") }}</label></td>
                                <td><input id="author" name="author" type="text" readonly required
                                        :value="story.author"></td>
                            </tr>

                            <tr>
                                <td><label for="private" :title="$t('storyPrivateDes')">{{ $t("storyPrivate") }}</label>
                                </td>
                                <td><input id="private" name="private" type="checkbox" checked></td>
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
            <div class="row mt-5" id="frames-row">
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">{{ $t('scenes') }}</h3>
                    <div class="row">
                        <div class="col-12 col-md-4"><input id="make-main" type="button" :value="$t('addScene')"
                                @click="addScene"></div>
                        <div class="col-12 col-md-4"><input id="view-frame" type="button" :value="$t('viewScene')">
                        </div>
                        <div class="col-12 col-md-4"><input id="make-main" type="button" :value="$t('makeMain')"
                                @click="makeMain"></div>
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
                    <SceneEditor :scenes="scenes" :currentSceneKey="currentSceneKey" :user-storage="userStorage">
                    </SceneEditor>
                </div>
            </div>
        </div>
    </section>
</template>