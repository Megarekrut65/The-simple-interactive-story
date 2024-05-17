<script setup>
import { v4 as uuid4 } from "uuid";
import { toRaw } from "vue";
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
    }
});

const id = uuid4();
const value = defineModel({ default: "" });

const onChanged = () => {
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
        <option v-for="data in list" :key="data.id" :value="data[valueKey]">{{ data[contentKey] }}
        </option>
    </datalist>
</template>