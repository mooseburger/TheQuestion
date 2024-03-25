<script>
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
            setStyle(newStyle) {
                document.body.classList.remove(styles.boring.id);
                document.body.classList.remove(styles.vajra.id);
                document.body.classList.remove(styles.gnosis.id);

                document.body.classList.add(newStyle.id);

                this.style = newStyle;
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
        <area shape="circle" coords="10,8,12" alt="Boring" @click="setStyle(styles.boring)" />
        <area shape="circle" coords="37,8,12" alt="Vajra" @click="setStyle(styles.vajra)" />
        <area shape="circle" coords="70,8,12" alt="Gnosis" @click="setStyle(styles.gnosis)" />
    </map>
</template>