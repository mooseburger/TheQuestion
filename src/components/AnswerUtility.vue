<script>
    export default {
        props: {
            id: Number,
            text: String,
            initialUpvoted: Boolean
        },
        emits: ['voteToggle'],
        data() {
            return {
                showShareOptions: false,
                upvoted: this.initialUpvoted
            }
        },
        methods: {
            async shareClick() {
                this.showShareOptions = true;
            },
            getTwitterMessage() {
                let tweet = this.text.substring(0, 231);
                if (this.text.length > 230) {
                    tweet += '...';
                }
                return encodeURIComponent(`${tweet}\n\n${window.location.origin}/answer/${this.id}`);
            },
            getWhatsAppMessage() {
                return encodeURIComponent(`${this.text}\n\n${window.location.origin}/answer/${this.id}`);
            },
            async copy() {
                const textWithLink = `${this.text}\n\n${window.location.origin}/answer/${this.id}`;
                await navigator.clipboard.writeText(textWithLink);

                const copiedToast = document.getElementById('copiedToast')

                if (copiedToast) {
                    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(copiedToast)
                    toastBootstrap.show();
                }
            },
            voteClick: async function () {
                if (!this.upvoted) {
                    await fetch(`/answer/vote/${this.id}/up`, { method: 'POST' });
                } else {
                    await fetch(`/answer/vote/${this.id}/undo`, { method: 'POST' });
                }

                this.upvoted = !this.upvoted;
                this.$emit('voteToggle', this.id);
            }
        }
    }
</script>

<template>
    <div class="row">
        <div class="col-12 col-md-4 col-lg-3 col-xl-2 d-flex">
            <a class="upvote me-4 answer-utility-option" @click="voteClick()">
                <i v-if="upvoted" class="bi bi-heart-fill"></i>
                <i v-if="!upvoted" class="bi bi-heart"></i>
            </a>
            <a class="share answer-utility-option" @click="shareClick()">
                <i class="bi bi-share"></i>
            </a>
        </div>
    </div>
    <div class="row mt-3" v-if="showShareOptions">
        <div class="col-12 col-md-4 col-lg-3 col-xl-2 d-flex justify-content-between">
            <a class="share-option copy-button" @click="copy">
                <i class="bi bi-copy"></i>
            </a>
            <a class="share-option twitter-share-button"
               target="_blank"
               :href="`https://twitter.com/intent/tweet?text=${getTwitterMessage()}`">
                <i class="bi bi-twitter-x"></i>
            </a>
            <a class="share-option whatsapp-share-button" target="_blank" :href="`whatsapp://send?text=${getWhatsAppMessage(a)}`">
                <i class="bi bi-whatsapp"></i>
            </a>
        </div>
    </div>
</template>