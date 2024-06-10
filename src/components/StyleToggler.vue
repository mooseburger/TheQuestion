<script>
    import cookieService from "../services/cookieService";

    const styleStorageName = 'TheQuestionStyle';

    const styles = {
        boring: { id: 'boring', name: 'Simple' },
        vajra: { id: 'vajra', name: 'Vajra' },
        gnosis: { id: 'gnosis', name: 'Gnosis' }
    };

    const pageContainer = document.getElementById('page-container');

    export default {
        data() {
            return {
                styles,
                style: styles.boring,
                initialPatternSize: null,
                zoomRate: 0,
                maxZoom: 0,
                popover: null
            }
        },
        methods: {
            vajraZoom() {
                if (!this.initialPatternSize) return;

                const scrollTop = window.scrollY || document.documentElement.scrollTop;
                let zoomLevel = this.initialPatternSize + (scrollTop / this.zoomRate); // Adjust the denominator to control the speed of zoom

                if (zoomLevel > this.maxZoom) zoomLevel = this.maxZoom;

                pageContainer.style.backgroundSize = `${zoomLevel}%, 100%`;
            },
            setStyle(newStyle, storeStyle = true, withAnimation = true) {
                if (this.popover) {
                    this.popover.dispose();
                    this.popover = null;
                }

                pageContainer.classList.remove(styles.boring.id, styles.vajra.id, styles.gnosis.id, 'fade-in');
                pageContainer.dataset.bsTheme = "";
                document.removeEventListener('scroll', this.vajraZoom);
                pageContainer.style.backgroundSize = '';
                pageContainer.classList.add(newStyle);

                if (newStyle === 'vajra') {
                    pageContainer.dataset.bsTheme = "dark";
                    const initialBackgroundSize = window.getComputedStyle(pageContainer).backgroundSize;
                    this.initialPatternSize = parseFloat(initialBackgroundSize.split(',')[0].split('%')[0]);

                    const currentBreakpoint = window.getComputedStyle(pageContainer).getPropertyValue('--current-breakpoint');
                    switch (currentBreakpoint) {
                        case 'xs':
                        case 'md':
                        case 'lg':
                            this.zoomRate = 500;
                            this.maxZoom = this.initialPatternSize * 1.25;
                            break;
                        case 'xl':
                        case 'xxl':
                            this.zoomRate = 300;
                            this.maxZoom = this.initialPatternSize * 2;
                            break;
                    }

                    document.addEventListener('scroll', this.vajraZoom);
                }

                // Trigger a reflow (thanks ChatGPT!)
                void document.body.offsetWidth;

                if (withAnimation) pageContainer.classList.add('fade-in');

                this.style = {
                    id: newStyle,
                    name: styles[newStyle].name
                };

                if (storeStyle) {
                    localStorage.setItem(styleStorageName, newStyle);
                    cookieService.setCookie(styleStorageName, newStyle, 365);
                }
            },
            async showPopovers() {
                await new Promise(r => setTimeout(r, 500)); // Wait for the component to have truly rendered. Hacky, but gets the job done...
                const config = {
                    content: 'Try a different look!',
                    trigger: 'manual',
                    container: 'body'
                }

                const currentBreakpoint = window.getComputedStyle(pageContainer).getPropertyValue('--current-breakpoint');
                if (currentBreakpoint !== 'xs') {
                    new bootstrap.Popover(document.getElementById('style-toggler'), {
                        ...config,
                        placement: 'bottom'
                    })
                    this.popover = bootstrap.Popover.getOrCreateInstance('#style-toggler');
                    this.popover.show();
                } else {
                    new bootstrap.Popover(document.querySelectorAll('.navbar-toggler')[0], {
                        ...config,
                        placement: 'left'
                    })
                    this.popover = bootstrap.Popover.getOrCreateInstance('.navbar-toggler');
                    this.popover.show();
                }
            }
        },
        created() {
            if (localStorage.getItem(styleStorageName)) {
                const style = localStorage.getItem(styleStorageName);
                this.setStyle(style, true, false);
            }
        },
        mounted() {
            if (!localStorage.getItem(styleStorageName)) {
                this.showPopovers();
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