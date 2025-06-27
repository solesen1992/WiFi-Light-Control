<!-- 
    This component displays the entire navigation bar in the app.
    It receives information about available pages (pages),
    which page is currently active (activePage), and what should happen
    when a link is clicked (navLinkClick).
-->

<template>
    <nav 
            :class="[`navbar-bg-dark`, `bg-light`, 'navbar', 'navbar-expand-lg']"  
            >
            <div class="container-fluid"> <!-- Makes navbar responsive and full width -->
                <a class="navbar-brand" href="#">
                    <img src="/assets/images/wexo.svg" alt="My Vue Logo" style="height: 35px;"> <!-- Logo that also functions as a home link -->
                </a>
                <!-- Navigation elements -->
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <!-- Loops through the pages array and displays a link for each page -->
                    <li v-for="(page, index) in pages" class="nav-item" :key="index">
                        <!-- Custom component: <navbar-link>
                        - page is passed as a prop so the link knows what to display
                        - isActive = true if the page is currently selected
                        - @click.prevent = triggers method on click but prevents default link behavior
                    -->
                        <navbar-link
                        :page="page"
                        :isActive="activePage === index"
                        @click.prevent="navLinkClick(index)"
                        ></navbar-link>
                    </li>
                </ul>
            </div>
        </nav>
</template>

<!-- JAVASCRIPT-->
<script>
// Import the child component that handles each link in the navbar
import navbarLink from './navbarLink.vue';

export default {
    components: {
        navbarLink // Makes the component available in this component
    },
            props: ['pages', 'activePage', 'navLinkClick'], // These props are controlled by the parent component
            data() {
                return {
                  // No local state in this component – everything is managed via props 
                }
            }
        }
</script>

<!-- CSS-->
<style>
    /* Makes it clear that navbar links are clickable */
    .navbar-nav .nav-link {
    cursor: pointer;
  }
</style>