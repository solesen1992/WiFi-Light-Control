// The class that starts the program (App.vue) and imports the root component and Bootstrap

// Import the createApp function from Vue, used to create the app instance
import { createApp } from 'vue'
// Import the root component – App.vue, which serves as the entry point for the entire Vue application
import App from './App.vue'
// Import Bootstrap CSS to make Bootstrap styling available throughout the app
import 'bootstrap/dist/css/bootstrap.css';

// Create a new Vue app based on the App component and "mount" it to the HTML element with id="app"
// This means App.vue will be rendered inside <div id="app"></div> in index.html
createApp(App).mount('#app')
