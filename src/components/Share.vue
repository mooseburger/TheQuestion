<script>
    export default {
        props: {
            id: Number,
            text: String
        },
        data() {
            return {
                showShareOptions: false
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
            copyLink() {


            }
        }
    }
</script>

<template>
    <button class="btn btn-outline-primary border border-0 share" @click="shareClick()">
        <svg width="27" height="23" viewBox="0 0 27 23" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
            <defs>
                <path id="qfy1ta9gfa" d="M0 0h27v23H0z" />
            </defs>
            <g fill="none" fill-rule="evenodd">
                <mask id="755deqlt3b" fill="#fff">
                    <use xlink:href="#qfy1ta9gfa" />
                </mask>
                <path d="M1.943 22.02c0 .541-.434.98-.971.98A.977.977 0 0 1 0 22.02c0-4.515 1.815-8.604 4.751-11.565a16.103 16.103 0 0 1 10.497-4.76V.978c0-.54.435-.979.971-.979.291 0 .552.13.731.334l9.761 9.643c.38.379.386.997.012 1.38-3.235 3.263-6.555 6.477-9.824 9.707a.964.964 0 0 1-1.37-.012.976.976 0 0 1-.278-.685l-.003-5.459c-3.663.127-6.944.961-9.358 2.245-2.438 1.296-3.947 3.02-3.947 4.868zM17.191 6.644V6.695l-.002.025h-.001l-.002.025-.002.024h-.001l-.003.025-.004.024-.005.025-.005.023-.005.024-.007.024-.007.023-.007.022-.007.023-.01.022-.008.022-.01.022-.01.021-.01.021-.011.022-.012.02-.011.021h-.001l-.012.02-.013.019-.014.02L17 7.23l-.015.02-.015.018-.015.018-.015.018-.016.017-.016.016-.017.017-.017.016-.018.016-.017.015-.018.014-.02.015-.019.014-.019.013-.019.013-.02.013-.02.012-.02.011h-.001l-.02.012-.021.01-.022.01-.022.01-.021.008-.022.01-.023.007-.023.008-.022.006-.023.007h-.001l-.023.005-.023.005h-.001l-.023.005-.025.004-.024.004-.024.003-.024.002h-.002l-.023.001-.025.002h-.026A14.172 14.172 0 0 0 6.125 11.84a14.41 14.41 0 0 0-3.198 4.918 13.348 13.348 0 0 1 2.06-1.338c2.885-1.534 6.849-2.485 11.208-2.489h.05l.025.002.023.001h.002l.024.001.024.004.024.003.025.005.023.004h.001l.023.005.023.005h.001l.023.008.022.006.023.007.023.009.022.009.021.007.022.01.022.011.02.01h.002l.019.012h.001l.02.012.02.011.02.012.02.014.018.013.02.014.019.014.018.016.017.014.018.015.017.017.017.017.016.016.016.017.015.018.015.017.014.02h.001l.015.017.012.02.014.019.013.02.012.02.012.02.012.021.01.02h.001l.01.023.01.02.01.022.009.022.009.023.007.022.007.023.007.024.007.023.005.023.005.024.005.025.004.023.003.025.003.024.002.026.002.024v.025h.001V18.039l7.457-7.366-7.457-7.367v3.338z" fill="currentColor" mask="url(#755deqlt3b)" />
            </g>
        </svg>
    </button>
    <div class="row mt-3" v-if="showShareOptions">
        <div class="col-12 col-md-4 col-lg-3 col-xl-2 d-flex justify-content-between">
            <a class="share-option copy-link-button" @click="copyLink()">
                <i class="bi bi-link-45deg"></i>
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