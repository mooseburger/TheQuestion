<script>
    import cookieService from "../services/cookieService";

    const styleStorageName = 'TheQuestionStyle';

    const styles = {
        boring: { id: 'boring', name: 'Boring' },
        vajra: { id: 'vajra', name: 'Vajra' },
        gnosis: { id: 'gnosis', name: 'Gnosis' }
    };

    export default {
        data() {
            return {
                styles,
                style: styles.boring
            }
        },
        methods: {
            setStyle(newStyle, storeStyle = true) {
                document.body.classList.remove(styles.boring.id);
                document.body.classList.remove(styles.vajra.id);
                document.body.classList.remove(styles.gnosis.id);

                document.body.classList.add(newStyle);

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
                const cycleWaitMillis = 1500;

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
                // Refresh the cookie
                cookieService.setCookie(styleStorageName, style, 365);
            }
        }
    }
</script>

<template>
    <img v-if="style.id === styles.boring.id" class="toggler" :alt="styles.boring.name" src="../assets/toggle-blue-left.svg" usemap="#style-toggler-map" />
    <img v-if="style.id === styles.vajra.id" class="toggler" :alt="styles.vajra.name" src="../assets/toggle-orange-center.svg" usemap="#style-toggler-map" />
    <img v-if="style.id === styles.gnosis.id" class="toggler" :alt="styles.gnosis.name" src="../assets/toggle-blue-right.svg" usemap="#style-toggler-map" />
    <span class="current-style">{{style.name}}</span>
    <map id="style-toggler-map" name="style-toggler-map">
        <area shape="circle" coords="10,8,12" alt="Boring" @click="setStyle(styles.boring.id)" />
        <area shape="circle" coords="37,8,12" alt="Vajra" @click="setStyle(styles.vajra.id)" />
        <area shape="circle" coords="70,8,12" alt="Gnosis" @click="setStyle(styles.gnosis.id)" />
    </map>
</template>