<script setup>
import BigBanner from '@/components/BigBanner.vue';
import LocalizedLink from '@/components/l10n/LocalizedLink.vue';
import StoryItem from '@/components/StoryItem.vue';

const image = new URL("@/assets/images/stories/post-1.jpg", import.meta.url).href;

</script>

<template>
    <BigBanner :title="$t('accountBanner')"></BigBanner>

    <section class="section">
        <div class="container">
            <div class="row">
                <div class="col-lg-10 mx-auto text-center">
                    <h2>{{ $t('yourStories') }}</h2>
                    <p>{{ $t('selectStory') }}</p>
                    <div class="section-border"></div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6 mb-4 m-1 add-container">
                    <LocalizedLink to="editor/new"><img src="@/assets/images/stories/plus.png" class="add-icon">
                    </LocalizedLink>
                </div>
                <StoryItem id="wdwdd" title="Example story" date="January 15, 2023" :image="image"></StoryItem>


            </div>
        </div>
    </section>
</template>