<script setup>
import { ref, watch } from 'vue';
import SafeDatalist from './SafeDatalist.vue';

const props = defineProps({
    items: {
        type: Array,
        required: true
    },
    onSelected: {
        type: Function,
        required: true
    },
    initial: {
        type: String,
        required: false,
        default: ""
    },
    fileType: {
        type: String,
        required: true
    },
    loadFunction: {
        type: Function,
        required: true
    },
    itemKey: {
        type: String,
        required: false,
        default: "item"
    },
    contentKey: {
        type: String,
        required: false,
        default: "id"
    },
    valueKey: {
        type: String,
        required: false,
        default: "id"
    }
});

const selectItem = ref(props.initial);

watch(() => props.initial, () => {
    selectItem.value = props.initial;
})

const item = ref(undefined);

const successLoad = (res, file) => {
    selectItem.value = "";
    item.value = { id: res.id, [props.itemKey]: res, name: res.name, file: file };

    props.onSelected(item.value);
};
const rejectLoad = (err) => {
    console.log(err);
    item.value = undefined;
    props.onSelected(undefined);
};
const onInputChanged = (event) => {
    props.loadFunction(event, successLoad, rejectLoad)
};

const onItemSelected = (value) => {
    item.value = value;

    props.onSelected(item.value);
};
</script>

<template>
    <div class="item-container">
        <SafeDatalist v-model="selectItem" :list="items" :content-key="contentKey" :value-key="valueKey"
            :on-select="onItemSelected" :initial="initial">
        </SafeDatalist>
        <input type="file" :accept="fileType" @change="onInputChanged">
    </div>
</template>
<style>
.item-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
}
</style>