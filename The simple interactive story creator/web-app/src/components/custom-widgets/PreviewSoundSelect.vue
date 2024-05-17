<script setup>
import { ref, watch } from 'vue';
import ItemSelect from './ItemSelect.vue';
import SingleSound from './SingleSound.vue';
import { loadSound } from '@/js/utilities/sound-utility';

const props = defineProps({
    sounds: {
        type: Array,
        required: true
    },
    onSelected: {
        type: Function,
        required: true
    },
    initial: {
        required: false,
        default: undefined
    }
});


const sound = ref(props.initial);

watch(() => props.initial, () => {
    sound.value = props.initial;
});

const onSoundSelected = (value) => {
    sound.value = value;
    props.onSelected(value);
};


</script>

<template>
    <ItemSelect :items="sounds" :on-selected="onSoundSelected" :initial="sound?.name" item-key="sound"
        content-key="name" value-key="name" file-type="audio/mp3" :load-function="loadSound">
    </ItemSelect>
    <SingleSound :sound="sound"></SingleSound>
</template>