import './scss/site.scss';
import './assets/boring-logo.svg';
import UserList from './components/UserList.vue';

import { createApp } from 'vue'

if (document.getElementById("user-list")) {
    createApp(UserList).mount('#user-list');
}
