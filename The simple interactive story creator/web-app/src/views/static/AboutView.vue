<script setup>
import BigBanner from '@/components/BigBanner.vue';
</script>

<template>
    <BigBanner :title="$t('title')" min-height="60vh"></BigBanner>
    <div class="basic-page">
        <section class="section">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="title-box">
                                    <h4 class="card-title">{{ $t('about') }}</h4>
                                </div>
                                <p class="card-text description-text">{{ $t('aboutDescription') }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </div>
</template>
<style>
.description-text {
    white-space: pre-wrap;
}
</style>