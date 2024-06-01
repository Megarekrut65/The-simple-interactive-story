<script setup>
import { v4 as uuid4 } from "uuid";
import { toRaw, watch } from "vue";
const props = defineProps({
    list: {
        type: Array,
        required: true
    },
    contentKey: {
        type: String,
        required: true
    },
    valueKey: {
        type: String,
        required: false,
        default: "id"
    },
    onSelect: {
        type: Function,
        required: true
    },
    required: {
        type: Boolean,
        required: false,
        default: false
    },
    initial: {
        type: String,
        required: false,
        default: ""
    },
    itemAction: {
        type: Object,
        required: false
    }
});

const id = uuid4();
const value = defineModel({ default: "" });
value.value = props.initial;

watch(() => props.initial, (newOne, oldOne) => {
    if (oldOne === value.value) value.value = newOne;
})


const onChanged = () => {
    if (props.itemAction && value.value === props.itemAction[props.valueKey]) {
        props.onSelect(toRaw(props.itemAction));
        value.value = props.initial;
        return;
    }
    const result = props.list.find(item => item[props.valueKey] === value.value);
    if (result === undefined) {
        value.value = "";
    }

    props.onSelect(toRaw(result));
};

</script>
<template>
    <input type="list" :list="id" :required="required" autocomplete="off" v-model="value" :placeholder="$t('selectOld')"
        @change="onChanged">
    <datalist :id="id">
        <option v-if="itemAction" :value="itemAction[valueKey]">{{ itemAction[contentKey] }}</option>
        <option v-for="data in list" :key="data.id" :value="data[valueKey]">{{ data[contentKey] }}
        </option>
    </datalist>
</template>