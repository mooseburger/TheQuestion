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
                statusId: null,
                sortDirection: 1,
                errors: null,
                pageNumber: 1,
                pageSize: 0,
                statuses: [],
                rejectedStatusId: 0
            }
        },
        watch: {
            statusId(newStatusId, oldStatusId) {
                this.getAnswerPage();
            },
            sortDirection(newSortDirection, oldSortDirection) {
                this.getAnswerPage();
            }
        },
        methods: {
            async getAnswerStatuses() {
                try {
                    const response = await fetch(`/answer/getStatuses`);
                    this.statuses = await response.json();
                    this.rejectedStatusId = this.statuses.find(s => s.name === 'Rejected').id;
                    this.statusId = this.statuses.find(s => s.name === 'In Review').id;
                } catch (err) {
                    this.errors = [err];
                }
            },
            async getAnswerPage() {
                try {
                    this.errors = null;
                    const response = await fetch(`/answer/getPage?statusId=${this.statusId}&sortDirection=${this.sortDirection}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`);
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

                if (!this.statuses.length)
                    return;

                await this.getAnswerPage();
            },
            async deleteAnswer(answer) {
                if (answer.statusId === this.rejectedStatusId)
                    return;

                this.errors = null;
                const confirmed = confirm(`Are you sure you want to delete answer ${answer.title}?`);
                if (!confirmed) {
                    return;
                }

                try {
                    this.errors = null;
                    const response = await fetch(`/answer/delete/${answer.id}`, { method: "DELETE" });
                    const result = await response.json();
                    if (!result.succeeded) {
                        this.errors = result.errors;
                        return;
                    }

                    await this.getAnswerPage();
                } catch (err) {
                    this.errors = [err];
                }
            }
        },
        created() {
            this.getAnswerStatuses();
        }
    }
</script>

<template>
    <div class="row mb-3">
        <div class="col-md-6">
            <p class="form-label">Status</p>
            <select class="form-select" v-model="statusId">
                <option value="">All Answers</option>
                <option :value="s.id" v-for="s in statuses">{{s.name}}</option>
            </select>
        </div>
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
                <th>Title</th>
                <th>Status</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="a in answers">
                <td>{{a.title}}</td>
                <td>{{a.statusName}}</td>
                <td>
                    <a :href="`/answer/edit/${a.id}`">Edit</a>
                    <a v-if="rejectedStatusId === a.statusId" class="ms-3" href="#" @click="deleteAnswer(a)">Delete</a>
                </td>
            </tr>
        </tbody>
    </table>
    <Paginator :total-records="totalRecords" @page-change="pageChange"/>
    <Alert :messages="errors"/>
</template>