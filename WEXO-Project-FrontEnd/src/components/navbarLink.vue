<!--
    This component renders a single link in the navigation menu.

    It receives two props:
    1. 'page' – contains information about the link (e.g., text and URL).
    2. 'isActive' – indicates whether this link points to the currently active page.

    If the link is active, extra CSS classes ('active' and 'emphasize') are added
    to visually highlight it (e.g., bold font).

    The link text and URL are dynamically set, making the component reusable
    across multiple pages.

    A computed property is used to determine the classes.
    This ensures better performance since the value is only recalculated
    when 'isActive' changes.
-->

<!--
    A reusable component that renders a single link in the navbar.
    It automatically highlights the link if it's active.
-->
<template>
    <a 
        class="nav-link"
        :class="activeClasses"
        aria-current="page" 
        :href="page.link.url"
        :title="`This link goes to ${page.link.text} page`"
    >{{ page.link.text }}</a>

</template>

<script>
    export default {
        props: ['page', 'isActive'], // Props: receives external data – 'page' holds the link text and URL
        computed: { // Computed property: returns CSS classes based on whether the link is active
            activeClasses() {
                return {
                    active: this.isActive,
                    emphasize: this.isActive // Adds 'emphasize' class if the link is active
                };
            }
        }
    }
</script>
