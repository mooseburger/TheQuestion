import './scss/site.scss';
import './assets/boring-logo.svg';
import UserList from './components/UserList.vue';
import AnswerDashboard from './components/AnswerDashboard.vue';

import { createApp } from 'vue'

const userListContainer = document.getElementById("user-list");
if (userListContainer) {
    const app = createApp(UserList);
    app.config.globalProperties.currentUsername = userListContainer.dataset.currentusername;
    app.mount(userListContainer);
}

const answerDashboardContainer = document.getElementById("answer-dashboard");
if (answerDashboardContainer) {
    const app = createApp(AnswerDashboard);
    app.config.globalProperties.isAdmin = answerDashboardContainer.dataset.isadmin.toLowerCase() === 'true';
    app.mount(answerDashboardContainer);
}
