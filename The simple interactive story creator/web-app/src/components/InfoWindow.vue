<script setup>
defineProps({
    title: {
        type: String,
        required: true
    },
    message: {
        type: String,
        required: true
    },
    okText: {
        type: String,
        required: false,
        default: null
    }
});

const isActive = defineModel();
const onClose = () => {
    isActive.value = false;
};
</script>

<template>
    <div v-if="isActive" class="question-modal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{ title }}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" @click="onClose">
                        <span aria-hidden="false">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>{{ message }}</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" @click="onClose">
                        {{ okText ? okText : $t('ok') }}</button>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.question-modal {
    top: 0;
    left: 0;
    position: fixed;
    z-index: 1999;
    width: 100vw;
    height: 100vh;

    background-color: rgba(0, 0, 0, 0.519);

}

.modal-dialog {
    margin-left: auto;
    margin-right: auto;
}
</style>