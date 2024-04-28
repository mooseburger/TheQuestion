
<script>
    import algoliasearch from 'algoliasearch/lite';
    import 'instantsearch.css/themes/algolia-min.css';
    import { AisInstantSearch, AisInfiniteHits, AisSearchBox, AisSortBy } from 'vue-instantsearch/vue3/es';
    import AnswerShare from './AnswerShare.vue';

    export default {
        components: {
            AisInstantSearch,
            AisInfiniteHits,
            AisSearchBox,
            AisSortBy,
            AnswerShare
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
            }
        }
    };
</script>

<template>
    <ais-instant-search :index-name="algolia.index"
                        :search-client="searchClient">
        <div class="row mb-5 answer-search-controls">
            <div class="col-md-9 col-lg-10">
                <ais-search-box :class-names="{ 'ais-SearchBox-input': 'form-control search-control', 'ais-SearchBox-submit': 'search-glass', 'ais-SearchBox-reset': 'search-reset' }" />
            </div>
            <div class="col-md-3 col-lg-2 mt-3 mt-md-0">
                <ais-sort-by :items="sort" :class-names="{ 'ais-SortBy-select': 'form-select search-control px-3' }"></ais-sort-by>
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
                                <answer-share :id="a.id" :text="a.text" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div v-if="!isLastPage" class="col-md-6 text-center text-md-end">
                        <button class="btn btn-question" @click="refineNext">Load More</button>
                    </div>
                    <div class="col-md-6 mt-3 mt-md-0 text-center text-md-end">
                        <button class="btn btn-question outline" @click="toTop">Back To Top</button>
                    </div>
                </div>
            </template>
        </ais-infinite-hits>
        
    </ais-instant-search>
</template>


