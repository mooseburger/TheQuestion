<script>
    export default {
        props: {
            totalRecords: Number
        },
        emits: ['pageChange'],
        data() {
            return {
                pageSize: 25,
                currentPage: 0,
                maxPagesToDisplay: 5,
                pages: [],
                totalPages: 0
            }
        },
        watch: {
            totalRecords(newTotalRecords, oldTotalRecords) {
                if (newTotalRecords !== oldTotalRecords) {
                    this.totalPages = Math.ceil(newTotalRecords / this.pageSize);

                    if (!this.pages.length) { // Initial setup of the paginator (we didn't know how many pages there were before this)
                        this.computePages(1, Math.min(this.totalPages, this.maxPagesToDisplay));
                    } else if (this.pages.at(-1) > this.totalPages) { // Reset the pages being displayed, since at least one no longer exsits
                        this.computePagesFromEnd(this.totalPages);
                    }
                }

                if (this.totalPages < this.currentPage) {
                    this.setPage(this.totalPages);
                }
            }
        },
        methods: {
            setPage(pageNumber) {
                if (pageNumber !== this.currentPage) {
                    if (pageNumber > this.pages.at(-1)) {
                        this.computePagesFromEnd(pageNumber);
                    } else if (pageNumber < this.pages[0]) {
                        this.computePagesFromStart(pageNumber);
                    }
                    this.currentPage = pageNumber > 0 ? pageNumber : 1;
                    this.$emit('pageChange', pageNumber, this.pageSize);
                }
            },
            computePagesFromEnd(endPage) {
                let startPage = Math.max(endPage - this.maxPagesToDisplay + 1, 1); // Ensures start is not less than 1
                this.computePages(startPage, endPage);
            },
            computePagesFromStart(startPage) {
                let endPage = Math.min(this.totalPages, startPage + this.maxPagesToDisplay - 1);
                this.computePages(startPage, endPage);
            },
            computePages(startPage, endPage) {
                let newPages = [];
                for (let i = startPage; i <= endPage; i++) {
                    newPages.push(i);
                }
                this.pages = newPages;
            }
        },
        created() {
            this.setPage(1);
        }
    }
</script>

<template>
    <div class="row">
        <div class="col-md-6" v-if="totalPages">
            <ul class="pagination" >
                <li class="page-item" :class="{ disabled: currentPage === 1}">
                    <a class="page-link" @click="setPage(1)">First</a>
                </li>
                <li class="page-item" :class="{ disabled: currentPage === 1}">
                    <a class="page-link" @click="setPage(currentPage - 1)">Previous</a>
                </li>
                <li class="page-item" :class="{ active: currentPage === p}" v-for="p in pages">
                    <a class="page-link" @click="setPage(p)">{{p}}</a>
                </li>
                <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                    <a class="page-link" @click="setPage(currentPage + 1)">Next</a>
                </li>
                <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                    <a class="page-link" @click="setPage(totalPages)">Last</a>
                </li>
            </ul>
        </div>
        <div class="col-md-6">
            <p class="mt-3">Total: {{totalRecords}}</p>
        </div>
    </div>
</template>