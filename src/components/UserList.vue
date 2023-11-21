<script>
    import Paginator from '/src/components/Paginator.vue';

    export default {
        components: {
            Paginator
        },
        data() {
            return {
                users: [],
                totalRecords: 0,
                error: null
            }
        },
        methods: {
            async getUserPage() {
                try {
                    const response = await fetch(`/admin/users?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`);
                    const paginatedResponse = await response.json();
                    this.users = paginatedResponse.page;
                    this.totalRecords = paginatedResponse.totalRecords;
                } catch (err) {
                    this.error = err;
                }
            },
            async pageChange(pageNumber, pageSize) {
                this.pageNumber = pageNumber;
                this.pageSize = pageSize;
                await this.getUserPage();
            }
        }
    }
</script>

<template>
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Username</th>
                <th>Email</th>
                <th>Role</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="u in users">
                <td>{{u.username}}</td>
                <td>{{u.email}}</td>
                <td>{{u.roleName}}</td>
            </tr>
            <tr v-if="error">
                <td>{{error}}</td>
            </tr>
        </tbody>
    </table>
    <Paginator :total-records="totalRecords" @page-change="pageChange"/>
</template>