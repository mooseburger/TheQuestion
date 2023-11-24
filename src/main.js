import './scss/site.scss';
import './assets/boring-logo.svg';
import UserList from './components/UserList.vue';

import { createApp } from 'vue'

const userListContainer = document.getElementById("user-list");
if (userListContainer) {
    const app = createApp(UserList)
    app.config.globalProperties.currentUsername = userListContainer.dataset.currentusername;
    app.mount(userListContainer);
}
