<script>
    import AnswerUtility from './AnswerUtility.vue';

    const answerUtilityContainer = document.getElementById("answer-utility");

    export default {
        components: {
            AnswerUtility
        },
        data() {
            return {
                id: answerUtilityContainer.dataset.id,
                text: answerUtilityContainer.dataset.text,
                upvoted: answerUtilityContainer.dataset.upvoted === "True",
            }
        },
        methods: {
            voteToggle: function () {
                this.upvoted = !this.upvoted;
            }
        }
    }
</script>

<template>
    <answer-utility :id="id" :text="text" :upvoted="upvoted" @vote-toggle="voteToggle" />
</template>