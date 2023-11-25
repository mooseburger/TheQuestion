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
                users: [],
                totalRecords: 0,
                errors: null
            }
        },
        methods: {
            async getUserPage() {
                try {
                    this.errors = null;
                    const response = await fetch(`/user/getPage?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`);
                    const paginatedResponse = await response.json();
                    this.users = paginatedResponse.page;
                    this.totalRecords = paginatedResponse.totalRecords;
                } catch (err) {
                    this.errors = [err];
                }
            },
            async pageChange(pageNumber, pageSize) {
                this.pageNumber = pageNumber;
                this.pageSize = pageSize;
                await this.getUserPage();
            },
            async deleteUser(username) {
                if (this.currentUsername === username) {
                    return;
                }

                const confirmed = confirm(`Are you sure you want to delete ${username}?`);
                if (!confirmed) {
                    return;
                }

                try {
                    this.errors = null;
                    const response = await fetch(`/user/delete/${username}`, { method: "DELETE" });
                    const result = await response.json();
                    if (!result.succeeded) {
                        this.errors = result.errors;
                        return;
                    }

                    await this.getUserPage();
                } catch (err) {
                    this.errors = [err];
                }
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
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="u in users">
                <td>{{u.username}}</td>
                <td>{{u.email}}</td>
                <td>{{u.roleName}}</td>
                <td>
                    <a :href="`/user/edit/${u.username}`">Edit</a>
                    <a v-if="currentUsername !== u.username" class="ms-3" href="#" @click="deleteUser(u.username)">Delete</a>
                </td>
            </tr>
        </tbody>
    </table>
    <Paginator :total-records="totalRecords" @page-change="pageChange"/>
    <Alert :messages="errors"/>
</template>