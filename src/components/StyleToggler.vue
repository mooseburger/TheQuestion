<script>
    import cookieService from "../services/cookieService";

    const styleStorageName = 'TheQuestionStyle';

    const styles = {
        boring: { id: 'boring', name: 'Boring' },
        vajra: { id: 'vajra', name: 'Vajra' },
        gnosis: { id: 'gnosis', name: 'Gnosis' }
    };

    function vajraZoom() {
        const scrollTop = window.scrollY || document.documentElement.scrollTop;
        let zoomLevel = 100 + (scrollTop / 80); // Adjust the denominator to control the speed of zoom

        if (zoomLevel > 205) zoomLevel = 205;

        document.body.style.backgroundSize = `${zoomLevel}%, 100%`;
    }

    export default {
        data() {
            return {
                styles,
                style: styles.boring
            }
        },
        methods: {
            setStyle(newStyle, storeStyle = true) {
                document.body.classList.remove(styles.boring.id, styles.vajra.id, styles.gnosis.id, 'fade-in');
                document.body.dataset.bsTheme = "";
                document.removeEventListener('scroll', vajraZoom);
                document.body.style.backgroundSize = '';
                document.body.classList.add(newStyle);

                if (newStyle === 'vajra') {
                    document.body.dataset.bsTheme = "dark";

                    document.addEventListener('scroll', vajraZoom);
                }

                // Trigger a reflow (thanks ChatGPT!)
                void document.body.offsetWidth;

                document.body.classList.add('fade-in');

                this.style = {
                    id: newStyle,
                    name: styles[newStyle].name
                };

                if (storeStyle) {
                    localStorage.setItem(styleStorageName, newStyle);
                    cookieService.setCookie(styleStorageName, newStyle, 365);
                }
            },
            async initializationCycle() {
                const cycleWaitMillis = 2000;

                await new Promise(r => setTimeout(r, cycleWaitMillis));

                this.setStyle(styles.vajra.id, false);

                await new Promise(r => setTimeout(r, cycleWaitMillis));

                this.setStyle(styles.gnosis.id, false);

                await new Promise(r => setTimeout(r, cycleWaitMillis));

                this.setStyle(styles.boring.id, true);
            }
        },
        created() {
            if (!localStorage.getItem(styleStorageName)) {
                this.initializationCycle();
            } else {
                const style = localStorage.getItem(styleStorageName);
                this.setStyle(style);
            }
        }
    }
</script>

<template>
    <div class="toggler-wrapper">
        <img v-if="style.id === styles.boring.id" class="toggler" :alt="styles.boring.name" src="../assets/toggle-blue-left.svg" usemap="#style-toggler-map" />
        <img v-if="style.id === styles.vajra.id" class="toggler" :alt="styles.vajra.name" src="../assets/toggle-orange-center.svg" usemap="#style-toggler-map" />
        <img v-if="style.id === styles.gnosis.id" class="toggler" :alt="styles.gnosis.name" src="../assets/toggle-blue-right.svg" usemap="#style-toggler-map" />
        <span class="current-style">{{style.name}}</span>
        <map id="style-toggler-map" name="style-toggler-map">
            <area shape="circle" coords="10,8,12" alt="Boring" @click="setStyle(styles.boring.id)" />
            <area shape="circle" coords="37,8,12" alt="Vajra" @click="setStyle(styles.vajra.id)" />
            <area shape="circle" coords="70,8,12" alt="Gnosis" @click="setStyle(styles.gnosis.id)" />
        </map>
    </div>
</template>