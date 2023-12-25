
<script>
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
    };
</script>

<template>
    <ais-instant-search :index-name="algolia.index"
                        :search-client="searchClient">
        <div class="row mb-4">
            <div class="col-md-10">
                <ais-search-box />
            </div>
            <div class="col-md-2">
                <ais-sort-by :items="sort"></ais-sort-by>
            </div>
        </div>
        
        <div class="row">
            <div class="col-12">
                <ais-infinite-hits>
                    <template v-slot="{ 
                        items,
                        refineNext,
                        isLastPage
                    }">
                        <div class="row mb-3" v-for="a in items">
                            <div class="col-12">
                                <div class="card answer">
                                    <div class="card-body">
                                        <h1 class="answer-title mb-5"><a :href="`/answer/${a.id}`">#{{ a.id }}</a></h1>
                                        <p class="answer-text">{{a.text}}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div v-if="!isLastPage" class="row justify-content-center">
                            <div class="col-md-6 text-center">
                                <button class="btn btn-question" @click="refineNext">Load More</button>
                            </div>
                        </div>
                    </template>
                </ais-infinite-hits>
            </div>
        </div>
        
    </ais-instant-search>
</template>


