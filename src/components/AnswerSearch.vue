
<script>
    import algoliasearch from 'algoliasearch/lite';
    import 'instantsearch.css/themes/algolia-min.css';
    import { AisInstantSearch, AisInfiniteHits, AisSearchBox } from 'vue-instantsearch/vue3/es';

    export default {
        components: {
            AisInstantSearch,
            AisInfiniteHits,
            AisSearchBox
        },
        data() {
            return {
                searchClient: algoliasearch(
                    this.algolia.appId,
                    this.algolia.apiKey
                ),
            };
        },
    };
</script>

<template>
    <ais-instant-search :index-name="algolia.index"
                        :search-client="searchClient">
        <ais-search-box />
        <ais-infinite-hits>
            <template v-slot:item="{ item }">
                <h2>#{{ item.id }}</h2>
                <p>{{item.text}}</p>
            </template>
        </ais-infinite-hits>
    </ais-instant-search>
</template>


