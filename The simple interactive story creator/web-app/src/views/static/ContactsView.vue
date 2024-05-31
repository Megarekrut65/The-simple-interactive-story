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
                                    <h4 class="card-title">{{ $t('contacts') }}</h4>
                                </div>
                                <p class="card-text"><strong>{{ $t('developer') }}</strong></p>
                                <p class="card-text">Бандалак Олександр</p>
                                <p class="card-text"><strong>{{ $t('email') }}</strong></p>
                                <p class="card-text">bandalak28@knu.ua</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </div>
</template>