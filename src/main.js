import './scss/site.scss';
import './assets/logo.svg';
import './assets/cloud.png';
import './assets/cloud-5.png';
import './assets/cloud-4.png';
import './assets/cloud-2.png';
import UserList from './components/UserList.vue';
import AnswerDashboard from './components/AnswerDashboard.vue';
import AnswerTable from './components/AnswerTable.vue';
import AnswerSearch from './components/AnswerSearch.vue';
import StyleToggler from './components/StyleToggler.vue';
import AnswerUtilityApp from './components/AnswerUtilityApp.vue';

import { createApp } from 'vue';

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

const answerTableContainer = document.getElementById("answer-table");
if (answerTableContainer) {
    const app = createApp(AnswerTable);
    app.mount(answerTableContainer);
}

const answerSearchContainer = document.getElementById("answer-search");
if (answerSearchContainer) {
    const app = createApp(AnswerSearch);
    app.config.globalProperties.algolia = {
        index: answerSearchContainer.dataset.index,
        appId: answerSearchContainer.dataset.appid,
        apiKey: answerSearchContainer.dataset.apikey
    };

    app.mount(answerSearchContainer);
}

const styleTogglerContainer = document.getElementById("style-toggler");
if (styleTogglerContainer) {
    const app = createApp(StyleToggler);
    app.mount(styleTogglerContainer);
}

const answerUtilityContainer = document.getElementById("answer-utility");
if (answerUtilityContainer) {
    const app = createApp(AnswerUtilityApp);
    app.mount(answerUtilityContainer);
}
