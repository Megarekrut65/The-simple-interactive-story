<script setup>
import BigBanner from '../BigBanner.vue';
import { ref } from 'vue';
import FrameEditor from './FrameEditor.vue';
import { textToId } from '@/js/text-utility';

const props = defineProps({
    storyId: {
        type: String,
        required: false
    }
});

const user = localStorage.getItem("userData");

const untitled = "Untitled";
const generate = "Generated automatically from story title";

const story = ref({
    id: props.storyId ? props.storyId : generate, title: untitled, preview: "Default", font: "Arial",
    author: user.displayName, private: true
});

const updateId = () => {
    if (story.value.title === "" || story.value.title === untitled) {
        story.value.id = generate;
        return;
    }

    story.value.id = textToId(story.value.title);
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
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">General story settings</h3>
                    <form onsubmit="return false;" action="#" :on-submit="submitStory">
                        <table class="form-table">
                            <tr>
                                <td><label class="star" for="identifier">Identifier</label></td>
                                <td><input v-model="story.id" name="identifier" type="text" required readonly
                                        value="Generated automatically from story title" style="width: 100%;"></td>
                            </tr>

                            <tr>
                                <td><label class="star" for="title">Title</label></td>
                                <td><input v-model="story.title" @input="updateId" name="title" type="text"
                                        placeholder="The simple story" required minlength="5" style="width: 100%;"></td>
                            </tr>

                            <tr>
                                <td><label for="preview">Preview</label></td>
                                <td>
                                    <div class="part-container">
                                        <input type="list" id="preview-saved" name="preview-saved"
                                            placeholder="Select old one..." autocomplete="off" list="preview-list">
                                        <datalist id="preview-list" class="image-list">
                                            <option>MyPreview.png</option>
                                        </datalist>
                                        <input id="preview" name="preview" type="file" accept="image/png">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td><label class="star" for="font">Font</label></td>
                                <td>
                                    <div class="part-container">
                                        <div style="color:black" id="font-example">Example</div>
                                        <select id="font" name="font">
                                            <option>
                                                Arial
                                            </option>
                                        </select>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td><label class="star" for="author">Author</label></td>
                                <td><input id="author" name="author" type="text" readonly required value="author"></td>
                            </tr>

                            <tr>
                                <td><label for="private"
                                        title="To view a private history, you can use its ID. Public stories can be found by name in the search">Keep
                                        private</label></td>
                                <td><input id="private" name="private" type="checkbox" checked></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>All mandatory fields are marked by <label class="star"></label></td>
                            </tr>
                            <tr>
                                <td><input type="submit" value="Save" style="margin-top: 20px;"></td>
                                <td></td>
                            </tr>
                        </table>

                    </form>
                </div>
            </div>
            <div class="row mt-5" id="frames-row">
                <div class="col mx-auto">
                    <h3 class="font-tertiary mb-5">Frames</h3>
                    <table>
                        <tr>
                            <td><input id="view-frame" type="button" value="View frame" style="margin-bottom: 40px;">
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td><input id="make-main" type="button" value="Make main" style="margin-bottom: 40px;">
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td><label class="star" for="frame">Frame</label></td>
                            <td>
                                <select id="frame" name="frame" type="list" required>
                                    <option>Main(main frame)</option>
                                </select>

                            </td>
                        </tr>
                    </table>
                    <FrameEditor></FrameEditor>
                </div>
            </div>
        </div>
    </section>
</template>