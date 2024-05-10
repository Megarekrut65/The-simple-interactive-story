<script setup>
import ImagesEditor from './ImagesEditor.vue';
</script>

<template>
    <ImagesEditor></ImagesEditor>
    <form onsubmit="return false;" action="#">
        <table class="form-table">
            <tr>
                <td><label class="star" for="title">{{ $t('sceneTitle') }}</label></td>
                <td><input id="title" name="title" type="text" placeholder="The main frame" value="Main" minlength="5"
                        required></td>
            </tr>

            <tr>
                <td><label class="star" for="background">{{ $t('sceneBackground') }}</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="background-saved" name="background-saved" :placeholder="$t('selectOld')"
                            autocomplete="off" list="background-list">
                        <datalist id="background-list" class="image-list">
                            <option>MyBackground.png</option>
                        </datalist>
                        <input id="background" name="background" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="background-music">{{ $t('sceneBackgroundMusic') }}</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="music-saved" name="music-saved" :placeholder="$t('selectOld')"
                            autocomplete="off" list="music-list">
                        <datalist id="music-list" class="music-list">
                            <option>MySound.mp3</option>
                        </datalist>
                        <input id="background-music" name="background-music" type="file" accept="audio/mp3">
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
                <td><label for="background-music">{{ $t('sceneAnswers') }}</label></td>
                <td>
                    <div>
                        <img id="answer-add" src="@/assets/images/stories/plus.png" class="add-icon answer-icon"
                            title="Add new answer">
                        <div id="answer-list" class="answer-list">

                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="left-image">{{ $t('sceneImages') }}</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="left-saved" name="left-saved" :placeholder="$t('selectOld')"
                            autocomplete="off" list="left-list">
                        <datalist id="left-list" class="image-list">
                            <option>Left.png</option>
                        </datalist>
                        <input id="left-image" name="left-image" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="auto-save" :title="$t('sceneAutoSaveDes')">{{ $t('sceneAutoSave') }}</label></td>
                <td><input id="auto-save" name="auto-save" type="checkbox" checked></td>
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