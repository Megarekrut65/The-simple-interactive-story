<script setup>
import { publishStory, storyExists, unpublishStory } from '@/js/firebase/story';
import { getRandomLetter, textToId } from '@/js/utilities/text-utility';
import { computed, ref } from 'vue';


const props = defineProps({
    story: {
        type: Object,
        required: true
    }
});

const story = computed(() => props.story);
const privateStory = ref(true);

const isActive = defineModel();
const onClose = () => {
    isActive.value = false;
};

const publishRecursive = (id) => {
    return storyExists(id).then(res => {
        if (res) return publishRecursive(id + getRandomLetter());
        const item = { id: id, storyId: story.value.id, authorId: story.value.authorId, private: privateStory.value, publishDate: new Date() };
        return publishStory(item).then(() => item);
    });
};

const onPublish = () => {
    let newId = textToId(story.value.author) + "-" + textToId(story.value.title);
    publishRecursive(newId).then(item => {
        story.value.publish = item.id;
    });
};

const onUnpublish = () => {
    unpublishStory(story.value.authorId, story.value.id, story.value.publish).then(() => {
        delete story.value.publish;
    });
};

</script>
<template>
    <div v-if="isActive" class="publish-modal">
        <div class="modal-dialog publish-dialog" role="document">
            <div v-if="story.publish" class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{ $t('publishTitleAlready') }}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" @click="onClose">
                        <span aria-hidden="false">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>{{ $t('publishDescriptionAlready') }}</p>
                    <p>{{ story.publish }}</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" @click="onClose">
                        {{ $t('ok') }}
                    </button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal" @click="onUnpublish">
                        {{ $t('unpublish') }}
                    </button>
                </div>
            </div>
            <div v-else class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{ $t('publishTitle') }}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" @click="onClose">
                        <span aria-hidden="false">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>{{ $t('publishDescription') }}</p>
                    <label :title="$t('storyPrivateDes')">{{ $t("storyPrivate") }}</label>
                    <input type="checkbox" checked v-model="privateStory">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" @click="onPublish">
                        {{ $t('publish') }}
                    </button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" @click="onClose">
                        {{ $t('cancel') }}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.publish-modal {
    top: 0;
    left: 0;
    position: fixed;
    z-index: 1998;
    width: 100vw;
    height: 100vh;

    background-color: rgba(0, 0, 0, 0.519);

}

.publish-dialog {
    position: fixed;
    top: 50%;
    left: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
}

.modal-body {
    white-space: pre-wrap;
}
</style>