<script>
    export default {
        props: {
            messages: Array
        }
    }
</script>

<template>
    <div class="alert alert-danger" v-if="messages">
        <ul>
            <li v-for="m in messages">{{m}}</li>
        </ul>
    </div>
</template>