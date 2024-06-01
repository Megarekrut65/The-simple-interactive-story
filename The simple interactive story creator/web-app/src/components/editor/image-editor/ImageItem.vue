<script setup>
import QuestionWindow from '@/components/QuestionWindow.vue';
import i18n from '@/i18n';
import { ref } from 'vue';

const props = defineProps({
    src: {
        type: String,
        required: true
    },
    onRemove: {
        type: Function,
        required: true
    },
    onUp: {
        type: Function,
        required: true
    },
    onDown: {
        type: Function,
        required: true
    },
    onSee: {
        type: Function,
        required: true
    }
})

const question = ref({
    title: "",
    question: "",
    active: false,
    yes: () => { }
});

const removeImage = () => {
    question.value = {
        title: i18n.t("removeImageTitle"),
        question: i18n.t("removeImageText"),
        active: true,
        yes: props.onRemove
    };
};
</script>

<template>
    <div class="image-container">
        <QuestionWindow v-model="question.active" :title="question.title" :question="question.question"
            :yes-function="question.yes"></QuestionWindow>
        <img :src="src">
        <div class="panel">
            <i class="fa-solid fa-eye text-primary custom-btn" @click="onSee" :title="$t('seeImage')"></i>
            <i class="fa-solid fa-circle-arrow-up text-success custom-btn" @click="onUp" :title="$t('upImage')"></i>
            <i class="fa-solid fa-circle-arrow-down text-warning custom-btn" @click="onDown"
                :title="$t('downImage')"></i>
            <i class="fa-solid fa-trash-can text-danger custom-btn" @click="removeImage" :title="$t('removeImage')"></i>
        </div>
    </div>
</template>

<style>
.image-container {
    box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
    margin: 5px;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.panel {
    display: flex;
    padding: 5px;
    justify-content: space-between;
}

.panel i {
    margin-left: 5px;
}
</style>