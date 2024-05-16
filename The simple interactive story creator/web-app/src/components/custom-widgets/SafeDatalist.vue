<script setup>
import { v4 as uuid4 } from "uuid";
import { ref } from "vue";
const props = defineProps({
    list: {
        type: Array,
        required: true
    },
    contentKey: {
        type: String,
        required: true
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
const value = ref("");

const onChanged = () => {
    const result = props.list.find(item => item.id === value.value);
    if (result === undefined) {
        value.value = "";
    }

    props.onSelect(value.value);
};

</script>
<template>
    <input type="list" :list="id" :required="required" autocomplete="off" v-model="value"
        :placeholder="$t('unselected')" @change="onChanged">
    <datalist :id="id">
        <option v-for="data in list" :key="data.id" :value="data.id">{{ data[contentKey] }}
        </option>
    </datalist>
</template>