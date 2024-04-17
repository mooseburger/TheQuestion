
<script>
    import { nextTick } from 'vue';
    import algoliasearch from 'algoliasearch/lite';
    import 'instantsearch.css/themes/algolia-min.css';
    import { AisInstantSearch, AisInfiniteHits, AisSearchBox, AisSortBy } from 'vue-instantsearch/vue3/es';

    export default {
        components: {
            AisInstantSearch,
            AisInfiniteHits,
            AisSearchBox,
            AisSortBy
        },
        data() {
            return {
                searchClient: algoliasearch(
                    this.algolia.appId,
                    this.algolia.apiKey
                ),
                sort: [
                    { value: this.algolia.index, label: 'Chronological' },
                    { value: `${this.algolia.index}_id_desc`, label: 'New first' }
                ]
            };
        },
        methods: {
            toTop: function () {
                window.scrollTo({
                    top: 0,
                    behavior: "smooth"
                });
            },
            async showShareOptions(answer) {
                answer.showShare = true;

                await nextTick();

                window.twttr = (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0],
                        t = window.twttr || {};
                    if (d.getElementById(id)) return t;
                    js = d.createElement(s);
                    js.async = false;
                    js.id = id;
                    js.src = "https://platform.twitter.com/widgets.js";
                    fjs.parentNode.insertBefore(js, fjs);

                    t._e = [];
                    t.ready = function (f) {
                        t._e.push(f);
                    };

                    return t;
                }(document, "script", "twitter-wjs"));

                if (window.twttr.widgets) {
                    window.twttr.widgets.load();
                }
            }
        }
    };
</script>

<template>
    <ais-instant-search :index-name="algolia.index"
                        :search-client="searchClient">
        <div class="row mb-5 answer-search-controls">
            <div class="col-md-9 col-lg-10">
                <ais-search-box :class-names="{ 'ais-SearchBox-input': 'form-control search-control py-2', 'ais-SearchBox-submit': 'search-glass', 'ais-SearchBox-reset': 'search-reset' }" />
            </div>
            <div class="col-md-3 col-lg-2 mt-3 mt-md-0">
                <ais-sort-by :items="sort" :class-names="{ 'ais-SortBy-select': 'form-select search-control px-3 py-2' }"></ais-sort-by>
            </div>
        </div>
        
        <ais-infinite-hits>
            <template v-slot="{
                items,
                refineNext,
                isLastPage
            }">
                <p v-if="!items?.length" class="no-results">No results</p>
                <div class="row mb-5" v-for="a in items">
                    <div class="col-12">
                        <div class="card answer">
                            <div class="card-body">
                                <h1 class="answer-title mb-4"><a :href="`/answer/${a.id}`">#{{ a.id }}</a></h1>
                                <p class="answer-text">{{a.text}}</p>
                                <button class="btn btn-outline-primary border border-0 share" @click="showShareOptions(a)">
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
                                <div v-if="a.showShare">
                                    <a class="twitter-share-button d-none"
                                       data-via="Example"
                                       data-size="medium"
                                       data-text="Check this out!"
                                       :data-url="`https://localhost:5175/answer/${a.id}`"
                                       href="https://twitter.com/intent/tweet">
                                        Tweet
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div v-if="!isLastPage" class="col-md-6 text-center text-md-end">
                        <button class="btn btn-question" @click="loadMore(refineNext)">Load More</button>
                    </div>
                    <div class="col-md-6 mt-3 mt-md-0 text-center text-md-end">
                        <button class="btn btn-question outline" @click="toTop">Back To Top</button>
                    </div>
                </div>
            </template>
        </ais-infinite-hits>
        
    </ais-instant-search>
</template>


