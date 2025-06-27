<!-- Controls which component is displayed -->

 <!-- HTML-->
<template>
    <div class="container">
      <!-- Displays the associated component -->
      <!-- `page.link.component` refers to a component imported below -->
      <!-- Vue's <component> tag is used to dynamically render different components -->
      <component :is="page.link.component" />
    </div>
  </template>
  
  <!-- JAVASCRIPT-->
  <script>
  // Imports the components the user can navigate between
  import macSettingsChoice from './macSettingsChoice.vue';
  import hostNameSettingsChoice from './hostNameSettingsChoice.vue';
  import descriptionSettingsChoice from './descriptionSettingsChoice.vue';
  import lightSettings from './lightSettings.vue';
  
  export default {
    // Receives a 'page' prop from the parent, which determines which component to render
    props: ['page'],
    // Registers the components so they can be used dynamically via <component :is="...">
    components: {
      macSettingsChoice,
      hostNameSettingsChoice,
      descriptionSettingsChoice,
      lightSettings
    }
  }
  </script>
  