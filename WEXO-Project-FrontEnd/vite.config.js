// Imports necessary functions and plugins, exports configurations.
// This file is important because it acts as the engine for Vue. It compiles Vue files 
// and JS, and starts the development server.

import { fileURLToPath, URL } from 'node:url' // Imports helper functions from Node.js to handle file paths

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools' 

// https://vite.dev/config/
// Exports the configuration for Vite – this file is automatically read by Vite at startup
export default defineConfig({
  // Specifies the plugins Vite should use
  plugins: [
    vue(), 
    vueDevTools(),
  ],
  // Configures how modules (files) can be imported
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
})
