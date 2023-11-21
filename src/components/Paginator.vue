<script>
    export default {
        props: {
            totalRecords: Number
        },
        emits: ['pageChange'],
        data() {
            return {
                pageSize: 25,
                currentPage: 0
            }
        },
        computed: {
            pages() {
                return [...Array(Math.ceil(this.totalRecords / this.pageSize)).keys()]
            }
        },
        watch: {
            totalRecords(newTotalRecords, oldTotalRecords) {
                if (Math.ceil(newTotalRecords / this.pageSize) < this.currentPage) {
                    this.setPage(this.pages.length);
                }
            }
        },
        methods: {
            setPage(pageNumber) {
                if (pageNumber !== this.currentPage) {
                    this.currentPage = pageNumber;
                    this.$emit('pageChange', pageNumber, this.pageSize);
                }
            }
        },
        created() {
            this.setPage(1);
        }
    }
</script>

<template>
    <ul class="pagination">
        <li class="page-item" :class="{ disabled: currentPage === 1}">
            <a class="page-link" @click="setPage(currentPage - 1)">Previous</a>
        </li>
        <li class="page-item" :class="{ active: currentPage === p + 1}" v-for="p in pages">
            <a class="page-link" @click="setPage(p + 1)">{{p + 1}}</a>
        </li>
        <li class="page-item" :class="{ disabled: currentPage === pages.length }" >
            <a class="page-link" @click="setPage(currentPage + 1)">Next</a>
        </li>
    </ul>
</template>