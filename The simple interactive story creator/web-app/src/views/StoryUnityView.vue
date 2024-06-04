<script setup>
import "../unity-assets/unity/TemplateData/style.css";
import BigBanner from "@/components/BigBanner.vue";
import { onMounted } from "vue";

//const assetsUrl = new URL("../unity-assets/unity", import.meta.url);
const assetsUrl = new URL("/assets/unity", import.meta.url);

const loadGame = () => {
    var container = document.querySelector("#unity-container");
    var canvas = document.querySelector("#unity-canvas");
    var loadingBar = document.querySelector("#unity-loading-bar");
    var progressBarFull = document.querySelector("#unity-progress-bar-full");
    var fullscreenButton = document.querySelector("#unity-fullscreen-button");
    var warningBanner = document.querySelector("#unity-warning");

    // Shows a temporary message banner/ribbon for a few seconds, or
    // a permanent error message on top of the canvas if type=='error'.
    // If type=='warning', a yellow highlight color is used.
    // Modify or remove this function to customize the visually presented
    // way that non-critical warnings and error messages are presented to the
    // user.
    function unityShowBanner(msg, type) {
        function updateBannerVisibility() {
            warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
        }
        var div = document.createElement('div');
        div.innerHTML = msg;
        warningBanner.appendChild(div);
        if (type == 'error') div.style = 'background: red; padding: 10px;';
        else {
            if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
            setTimeout(function () {
                warningBanner.removeChild(div);
                updateBannerVisibility();
            }, 5000);
        }
        updateBannerVisibility();
    }

    var buildUrl = assetsUrl.href + "/Build";
    var loaderUrl = buildUrl + "/Web.loader.js";
    var config = {
        dataUrl: buildUrl + "/Web.data",
        frameworkUrl: buildUrl + "/Web.framework.js",
        codeUrl: buildUrl + "/Web.wasm",
        streamingAssetsUrl: assetsUrl.href + "StreamingAssets",
        companyName: "BOAGames",
        productName: "The simple interactive story",
        productVersion: "2.1",
        showBanner: unityShowBanner,
    };

    // By default, Unity keeps WebGL canvas render target size matched with
    // the DOM size of the canvas element (scaled by window.devicePixelRatio)
    // Set this to false if you want to decouple this synchronization from
    // happening inside the engine, and you would instead like to size up
    // the canvas DOM size and WebGL render target sizes yourself.
    // config.matchWebGLToCanvasSize = false;

    if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        // Mobile device style: fill the whole browser client area with the game canvas:

        var meta = document.createElement('meta');
        meta.name = 'viewport';
        meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
        document.getElementsByTagName('head')[0].appendChild(meta);
        container.className = "unity-mobile";
        canvas.className = "unity-mobile";

        // To lower canvas resolution on mobile devices to gain some
        // performance, uncomment the following line:
        // config.devicePixelRatio = 1;


    }

    canvas.style.background = "url('" + buildUrl + "/TemplateData.jpg') center / cover";
    loadingBar.style.display = "block";

    var script = document.createElement("script");
    script.src = loaderUrl;
    script.onload = () => {
        // eslint-disable-next-line no-undef
        createUnityInstance(canvas, config, (progress) => {
            progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
            loadingBar.style.display = "none";
            fullscreenButton.onclick = () => {
                unityInstance.SetFullscreen(1);
            };
        }).catch((message) => {
            alert(message);
        });
    };

    document.body.appendChild(script);

};

onMounted(loadGame);

</script>

<template>
    <BigBanner min-height="100vh"></BigBanner>
    <div class="not-supported">
        <p class="text-white text-center"> {{ $t('mobileSupport') }}
        </p>
    </div>
    <div class="not-supported">
        <div id="unity-container" class="unity-desktop canvas-container">
            <canvas id="unity-canvas" width="1920" height="1080" tabindex="-1" style="width: 80%;"></canvas>
            <div class="mt-4">
                <div id="unity-loading-bar">
                    <div id="unity-logo"></div>
                    <div id="unity-progress-bar-empty">
                        <div id="unity-progress-bar-full"></div>
                    </div>
                </div>
                <div id="unity-warning"> </div>
                <div id="unity-footer">
                    <div id="unity-webgl-logo"></div>
                    <div id="unity-fullscreen-button"></div>
                    <div id="unity-build-title">The simple interactive story</div>
                </div>
            </div>
        </div>
    </div>

</template>
<style>
.not-supported {
    position: absolute;
    left: 0;
    top: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
    z-index: 100;
}

#unity-canvas {
    z-index: 101;
}

.not-supported>p {
    width: 60%;
}

.canvas-container {
    width: 80%;
    display: flex;
    justify-content: space-around;
    flex-direction: column;
    align-items: center;
    height: 100%;
}
</style>