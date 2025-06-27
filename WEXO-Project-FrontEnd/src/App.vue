 <!-- Root component of the Vue app. Manages navigation (navbar) and login -->
 
 <!-- HTML-->
<template>
  <div>
    <!-- Shows login screen if the user is not authenticated -->
    <login v-if="!isAuthenticated" @login-success="onLoginSuccess" />

    <!-- Displays the main app content once the user is logged in -->
    <div v-else>
      <!-- Navbar with page selection -->
      <navbar
        :pages="pages"
        :active-page="activePage"
        :nav-link-click="(index) => activePage = index"
      />

      <!-- Only shows the currently active page inside the page viewer -->
      <page-viewer
        v-if="pages.length > 0"
        :page="pages[activePage]" 
      ></page-viewer> <!-- Correct closing tag for page-viewer -->
    </div>
  </div>
</template>

<!-- JAVASCRIPT-->
<script>
// Import all necessary Vue components that this root component depends on
import login from './components/login.vue';
import navbar from './components/navbar.vue';
import pageViewer from './components/pageViewer.vue';
import macSettingsChoice from './components/macSettingsChoice.vue';
import hostNameSettingsChoice from './components/hostNameSettingsChoice.vue';
import descriptionSettingsChoice from './components/descriptionSettingsChoice.vue';
import lightSettings from './components/lightSettings.vue';

export default {
  // Register the imported components so they can be used in the template
  components: {
    login,
    navbar,
    pageViewer,
    macSettingsChoice,
    hostNameSettingsChoice,
    descriptionSettingsChoice,
    lightSettings
  },
  data() {
    return {
      isAuthenticated: false, // Tracks whether the user is logged in
      activePage: 0, // Start with the first page (index 0). Index for the active page.
      pages: [], // Stores the pages fetched from pages.json. Saves the list of pages.
    };
  },
  // When the component is created, check if the user was previously logged in
  created() {
    // Check if the user is already authenticated (e.g., after a previous login)
    if (localStorage.getItem('authenticated')) {
      this.isAuthenticated = true; // Set status to logged in
      this.getPages(); // Load pages after login
    }
  },
  methods: {
    // Asynchronous function to fetch page names and info from a JSON file
    async getPages() {
      const res = await fetch('pages.json'); // Fetch the JSON file with page data
      const data = await res.json(); // Parse JSON-data
      this.pages = data;
      this.activePage = 0; // Set the starting page to index 0 after login
    },
    // Called when login is successful
    onLoginSuccess() {
      // Store authentication state after login success
      localStorage.setItem('authenticated', 'true'); // Store authentication state in localStorage
      this.isAuthenticated = true; // Set status to logged in
      this.getPages(); // Load pages after login
    }
  }
};
</script>

