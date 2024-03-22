<script>
    const styles = {
        boring: 'boring',
        vajra: 'vajra',
        gnosis: 'gnosis'
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
                document.body.classList.remove(styles.boring);
                document.body.classList.remove(styles.vajra);
                document.body.classList.remove(styles.gnosis);

                document.body.classList.add(newStyle);

                this.style = newStyle;
            }
        }
    }
</script>

<template>
    <img v-if="style === styles.boring" src="../assets/toggle-blue-left.svg" usemap="#style-toggler-map" />
    <img v-if="style === styles.vajra" src="../assets/toggle-orange-center.svg" usemap="#style-toggler-map" />
    <img v-if="style === styles.gnosis" src="../assets/toggle-blue-right.svg" usemap="#style-toggler-map" />
    <map id="style-toggler-map" name="style-toggler-map">
        <area shape="circle" coords="10,8,12" alt="Boring" @click="setStyle(styles.boring)" />
        <area shape="circle" coords="37,8,12" alt="Vajra" @click="setStyle(styles.vajra)" />
        <area shape="circle" coords="70,8,12" alt="Gnosis" @click="setStyle(styles.gnosis)" />
    </map>
</template>