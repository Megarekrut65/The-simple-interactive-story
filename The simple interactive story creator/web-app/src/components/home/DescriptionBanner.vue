<script setup>
import DescriptionItem from "./DescriptionItem.vue";
import { computed } from "vue";
import i18n from "@/i18n";
import LocalizedLink from "../l10n/LocalizedLink.vue";

const items = computed(() => [
    {
        title: i18n.t("desSceneTitle"),
        description: i18n.t("desSceneDescription")
    },
    {
        title: i18n.t("desImageTitle"),
        description: i18n.t("desImageDescription")
    },
    {
        title: i18n.t("desSoundTitle"),
        description: i18n.t("desSoundDescription")
    },
    {
        title: i18n.t("desShareTitle"),
        description: i18n.t("desShareDescription")
    }
]);
</script>

<template>
    <section class="section bg-secondary position-relative">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <div class="row align-items-center">
                        <div class="col-lg-5 mb-4 lg-mb-0">
                            <img src="@/assets/images/logo.png" alt="feature-image" class="img-fluid">
                        </div>
                        <div class="col-lg-7">
                            <div class="row">
                                <div class="col-12">
                                    <h3 class="text-white">{{ $t('descriptionTitle') }}</h3>
                                    <div class="section-border ml-0"></div>
                                </div>
                                <div class="col-md-6 mb-4" v-for="item in items" :key="item.title">
                                    <DescriptionItem :title="item.title" :description="item.description">
                                    </DescriptionItem>
                                </div>
                                <div class="col-12">
                                    <LocalizedLink to="/editor/new" class="btn btn-primary custom-btn text-white">{{
                                        $t('go') }}
                                    </LocalizedLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>
