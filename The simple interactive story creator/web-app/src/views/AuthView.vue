<script setup>
import SingIn from '@/components/auth/SingIn.vue';
import SingUp from '@/components/auth/SingUp.vue';
import BigBanner from '@/components/BigBanner.vue';

</script>
<template>
    <BigBanner :title="$t('authBanner')"></BigBanner>
    <section class="section">
        <div class="container">
            <div class="row">
                <div class="col-lg-6 col-12">
                    <SingIn></SingIn>
                </div>

                <div class="col-lg-6 col-12">
                    <SingUp></SingUp>
                </div>
            </div>
        </div>
    </section>
</template>