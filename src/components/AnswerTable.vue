<script>
    import Paginator from '/src/components/Paginator.vue';
    import Alert from '/src/components/Alert.vue';

    export default {
        components: {
            Paginator,
            Alert
        },
        data() {
            return {
                answers: [],
                totalRecords: 0,
                sortDirection: 2,
                errors: null,
                pageNumber: 1,
                pageSize: 0,
            }
        },
        watch: {
            sortDirection(newSortDirection, oldSortDirection) {
                this.getAnswerPage();
            }
        },
        methods: {
            async getAnswerPage() {
                try {
                    this.errors = null;
                    const response = await fetch(`/answer/getTablePage?sortDirection=${this.sortDirection}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`);
                    const paginatedResponse = await response.json();
                    this.answers = paginatedResponse.page;
                    this.totalRecords = paginatedResponse.totalRecords;
                } catch (err) {
                    this.errors = [err];
                }
            },
            async pageChange(pageNumber, pageSize) {
                this.pageNumber = pageNumber;
                this.pageSize = pageSize;

                await this.getAnswerPage();
            },
            async reindexAnswer(id) {
                try {
                    this.errors = null;
                    const response = await fetch(`/answer/reindex/${id}`, { method: 'POST' });
                } catch (err) {
                    this.errors = [err.message];
                }
            }
        }
    }
</script>

<template>
    <div class="row mb-3">
        <div class="col-md-6">
            <p class="form-label">Sort</p>
            <select class="form-select" v-model="sortDirection">
                <option value="1">Ascending</option>
                <option value="2">Descending</option>
            </select>
        </div>
    </div>    
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Id</th>
                <th>Text</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="a in answers">
                <td>{{a.id}}</td>
                <td>{{a.text}}</td>
                <td>
                    <a :href="`/answer/view/${a.id}`">View</a>
                    <a class="ms-3" href="#" @click="reindexAnswer(a.id)">Re-index</a>
                </td>
            </tr>
        </tbody>
    </table>
    <Paginator :total-records="totalRecords" @page-change="pageChange"/>
    <Alert :messages="errors"/>
</template>