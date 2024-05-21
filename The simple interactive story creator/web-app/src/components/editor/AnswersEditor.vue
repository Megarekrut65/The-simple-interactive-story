<script setup>
import { computed } from 'vue';
import SafeDatalist from '../custom-widgets/SafeDatalist.vue';
import { removeAt } from '@/js/utilities/array-utility';


const props = defineProps({
    answers: {
        type: Array,
        required: true
    },
    scenes: {
        type: Object,
        required: true
    },
    onUpdate: {
        type: Function,
        required: true
    }
});


const scenes = computed(() => Object.values(props.scenes));
const answers = computed(() => props.answers);

const onSceneSelect = (index, value) => {
    if (index >= 0 && index < answers.value.length) {
        answers.value[index].nextScene = value.id;
        props.onUpdate(answers.value);
    }
};
const onRemove = (index) => {
    removeAt(answers.value, index);
    props.onUpdate(answers.value);
};

</script>
<template>
    <div class="row">
        <div class="col-12 answer" v-for="(data, index) in answers" :key="data.id">
            <table>
                <tr>
                    <td><label class="star">{{ $t('text') }}</label></td>
                    <td>
                        <textarea style="width: 100%;" :placeholder="$t('answerHint')" v-model="answers[index].text"
                            required></textarea>
                    </td>
                </tr>
                <tr>
                    <td><label class="star">{{ $t('nextScene') }}</label></td>
                    <td>
                        <SafeDatalist :list="scenes" :on-select="(value) => onSceneSelect(index, value)"
                            :required="true" content-key="title" value-key="title"></SafeDatalist>
                    </td>
                </tr>
            </table>
            <div class="options-container">
                <i class="fa-solid fa-sliders custom-btn text-waring" :title="$t('advancedOptions')"></i>
                <i class="fa-solid fa-x custom-btn text-danger" @click="() => onRemove(index)"
                    :title="$t('removeAnswer')"></i>

            </div>

        </div>
    </div>
</template>
<style>
.answer {
    border: 1px solid black;
    display: flex;
    justify-content: space-between;
    padding-top: 10px;
    margin-bottom: 10px;
    background-color: rgba(0, 0, 0, 0.067);
}

.options-container {
    display: flex;
}

.options-container i {
    margin-left: 15px;
}
</style>