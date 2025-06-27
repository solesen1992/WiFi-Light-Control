 <!-- Creates the hostNameSettingsChoice page and performs the necessary API calls -->

<!-- HTML -->
<template>
  <!-- Page header with title and description -->
    <div class="container mb-5">
        <h1>
        Vedligeholdelse af lys-indstillinger
    </h1>
    <h5>
        Tilføj host names som skal frasorteres af systemet
    </h5>
    </div>

    <!-- Form for adding a new host name -->
    <div>
        <form @submit.prevent="addItem">
            <input v-model="newItem.hostName" placeholder="Host Name" required/>
            <button class="addButton" type="Submit">Tilføj</button>
        </form>

    <!-- Error Popup -->
    <!-- Displayed when something goes wrong -->
    <div v-if="isErrorPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Error!</h3>
        <p>Der opstod et problem, venligst prøv igen</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

     <!-- Success Popup -->
    <!-- Displayed when a host name is successfully added -->
     <div v-if="isSuccessPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Success!</h3>
        <p>Host name "{{ addedHostName }}" er blevet tilføjet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Success Popup for Deletion -->
    <!-- Displayed when a host name is deleted -->
    <div v-if="isDeletePopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Deleted!</h3>
        <p>Host name "{{ deletedHostName }}" er blevet slettet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Table displaying existing host names -->
        <table>
            <thead>
                <tr>
                    <th>Host name</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in items" :key="index">
                    <td>{{ item.hostName }}</td>
                    <td>
                        <!-- Delete button for each host name -->
                        <button @click="removeItem(index)" class="delete">Slet</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    </template>


<!-- JAVASCRIPT-->
<script>

import { BASE_URI } from '@/components/baseURIconfig.js';

export default {
  props: ['page'],
  data() {
    return {
      newItem: {
        hostName: '' // User input for new host name
      },
      items: [], // List of all existing host names
      // Popup control
      isSuccessPopupOpen: false,
      isErrorPopupOpen: false,
      isDeletePopupOpen: false,
      // Used to display the added/deleted host name
      deletedHostName: '',
      addedHostName: ''
    };
  },
  mounted() { // Runs when the component is mounted - fetches existing host names
    this.loadItems();
  },
  methods: {
    // Add a new item to the list
    // POST request to API to add new host name
    async addItem() {
  try {
    if (this.newItem.hostName) {
      const response = await fetch(`${BASE_URI}/Blacklist/hostname`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          hostName: this.newItem.hostName
        })
      });

      if (!response.ok) {
        throw new Error('Error with connecting and posting item');
      }

      // Successfully added the item
      this.items.push({
        hostName: this.newItem.hostName
      });

      this.addedHostName = this.newItem.hostName;

      // Clear the input field
      this.newItem.hostName = '';

      // Show success popup
      this.isSuccessPopupOpen = true;

    }
  } catch (error) {
    console.error('Failed to post item:', error);

    // Show error popup if something goes wrong
    this.isErrorPopupOpen = true;

  }
},

    // Fetch data from API
    // GET-request for fetching existing host names from API
    async loadItems() {
      try {
        const response = await fetch(`${BASE_URI}/Blacklist/hostname`);
        const data = await response.json();
        console.log("Fetched data:", data);
        this.items = data;
      } catch (error) {
        console.error('Failed to fetch items:', error);
      }
    },

    // Remove an item based on its index
    // DELETE request to remove host name
    async removeItem(index) {
      try {
        const item = this.items[index];

        if (!item || !item.hostName) {
          throw new Error("Item not found or has no host name");
        }

        const response = await fetch(`${BASE_URI}/Blacklist/hostname/${encodeURIComponent(item.hostName)}`, {
          method: 'DELETE'
        });

        if (!response.ok) {
          throw new Error('Failed to delete description');
        }

        // Successfully deleted the item, remove from array
        this.items.splice(index, 1);

        this.deletedHostName = item.hostName;

        // Show delete success popup
        this.isDeletePopupOpen = true;

        // Close the success popup after 3 seconds
      } catch (error) {
        console.error(error);

        // Show error popup if deletion fails
        this.isErrorPopupOpen = true;
    
      }
    },
    // Close all popups
    closePopup() {
      this.isSuccessPopupOpen = false;
      this.isErrorPopupOpen = false;
      this.isDeletePopupOpen = false;
  }
}
}
</script>

<!-- CSS -->
<style scoped>
/* Basic styling for popup */
/* Popup background */
.popup-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* Semi-transparent black */
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Popup content */
.popup-content {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  width: 300px;
  text-align: center;
}

/* Container styling */
.container {
  max-width: 700px;
  margin: 2rem auto;
  background-color: #fff;
  padding: 2rem;
}

/* Typography for headings */
h1 {
  font-size: 2rem;
  font-weight: 600;
  margin-bottom: 1.5rem;
}

h5 {
  font-size: 1rem;
  font-weight: 400;
  margin-bottom: 1.5rem;
  color: #555;
}

/* Form layout */
form {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
  max-width: 1280px; /* or whatever width you want */
  flex-wrap: wrap;  /* if items should wrap on small screens */
}

/* Input field styling */
input {
  flex: 1;
  padding: 0.75rem;
  font-size: 1rem;
  border: 2px solid #ccc;
  border-radius: 8px;
  outline: none;
  transition: border-color 0.3s ease;
}

input:focus {
  border-color: #7d4dfa;
}

/* Add button */
button[type="submit"] {
  background-color: #7d4dfa;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s ease;
  margin-left: 0;
}

button[type="submit"]:hover {
  background-color: #6a3fd1;
}

/* Table styling */
table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  border: 1px solid #ccc;
  border-radius: 8px;
  overflow: hidden;
}

thead {
  background-color: #d4d3d3;
  font-weight: bold;
}

/* Cells */
th, td {
  border-bottom: 1px solid #ccc;
  border-right: 1px solid #ccc;
  padding: 1rem;
  text-align: left;
}

/* Last column for buttons */
td:last-child, th:last-child {
  width: 100px; /* or whatever fits your button nicely */
  text-align: center;
}

td button {
  width: 80px;         /* consistent button width */
  padding: 0.5rem 0;   /* top-bottom padding only */
  border: none;
  background-color: #e11d48;
  color: white;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
}

thead th {
  border: none;
}

/* Delete button styling */
button.delete {
  background-color: #e81d5b;
  color: white;
  padding: 0.5rem 1.2rem;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
}

button.delete:hover {
  background-color: #c9164c;
}

/* Logo size  */
.logo {
    max-width: 300px;
}
</style>