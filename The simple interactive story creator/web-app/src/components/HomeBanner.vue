<script setup>
const image = new URL('@/assets/images/banner/banner2.jpg', import.meta.url).href;
</script>

<template>
    <section class="banner bg-cover position-relative d-flex justify-content-center align-items-center" 
    :style="{ backgroundImage: 'url(' + image + ')' }">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="display-1 text-white font-weight-bold font-primary">The simple interactive story</h1>
                </div>
            </div>
        </div>
    </section>
</template>