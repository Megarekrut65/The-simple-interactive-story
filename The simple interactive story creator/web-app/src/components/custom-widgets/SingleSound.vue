<script setup>
import { soundToSrc } from '@/js/utilities/sound-utility';

defineProps({
    sound: {
        type: [Audio, String, Object],
        required: false
    }
});

</script>

<template>
    <audio :key="sound" controls>
        <source :src="soundToSrc(sound)" type="audio/mp3">
        {{ $t('notSupportAudio') }}
    </audio>

</template>