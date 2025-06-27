<!-- Creates the descriptionSettingsChoice page and performs the necessary API calls -->
<!-- Component for maintaining the description blacklist -->

<!-- HTML -->
<template>

    <!-- Heading and introduction -->
    <div class="container mb-5">
        <h1>
        Vedligeholdelse af lys-indstillinger
    </h1>
    <h5>
        Tilføj descriptions som skal frasorteres af systemet
    </h5>
    </div>
    <!-- Form for adding a new description -->
    <div>
        <form @submit.prevent="addItem">
            <input v-model="newItem.description" placeholder="Description" required/>
            <button class="addButton" type="Submit">Tilføj</button>
        </form>

    <!-- Popup: Error message -->
    <div v-if="isErrorPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Error!</h3>
        <p>Der opstod et problem, venligst prøv igen</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

     <!-- Popup: Success on addition -->
     <div v-if="isSuccessPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Success!</h3>
        <p>Description "{{ addedDescription }}" er blevet tilføjet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Popup: Success on deletion -->
    <div v-if="isDeletePopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Deleted!</h3>
        <p>Description "{{ deletedDescription }}" er blevet slettet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Table of existing descriptions -->
        <table>
            <thead>
                <tr>
                    <th>Description</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in items" :key="index">
                    <td>{{ item.description }}</td>
                    <td>
                        <button @click="removeItem(index)" class="delete">Slet</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    </template>

<!-- Javascript -->
<script>

// Base URL for API calls
import { BASE_URI } from '@/components/baseURIconfig.js';

export default {
  props: ['page'], // Receives a page prop
  data() {
    return {
      newItem: { 
        description: '' // The description to be added
      },
      items: [], // List of existing descriptions
      isSuccessPopupOpen: false, // Controls visibility of success popup
      isErrorPopupOpen: false, // Controls visibility of error popup
      isDeletePopupOpen: false, // Controls visibility of delete popup
      deletedDescription: '', // Stores the deleted description
      addedDescription: '' // Stores the newly added description
    };
  },
  mounted() {
    this.loadItems(); // Loads existing descriptions on initialization
  },
  methods: {
    // Adds a new description via POST request
    async addItem() {
        try {

            if (this.newItem.description) {
                const response = await fetch(`${BASE_URI}/Blacklist/description`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    description: this.newItem.description
                })
            });

        if (!response.ok) {
            throw new Error('Error with connecting and posting item')
        }

        // Add the new description to the local list
        this.items.push({
          description: this.newItem.description
        });

        this.addedDescription = this.newItem.description;

        // Clear the input field
        this.newItem.description = '';

        this.isSuccessPopupOpen = true;
      }
        } catch (error) {
            console.error('Failed to post item:', error)
            this.isErrorPopupOpen = true;
        }
    },

    // Loads all descriptions from the API
    async loadItems() {
      try {
        const response = await fetch(`${BASE_URI}/Blacklist/description`);
        const data = await response.json();
        console.log("Fetched data:", data);
        this.items = data;
      } catch (error) {
        console.error('Failed to fetch items:', error);
      }
    },

    // Removes an item from the table by index
    // Deletes a description via DELETE request
    async removeItem(index) {
      try {
        const item = this.items[index];

        if (!item || !item.description) {
          throw new Error("Item not found or has no description");
        }

        const response = await fetch(`${BASE_URI}/Blacklist/description/${encodeURIComponent(item.description)}`, {
          method: 'DELETE'
        });

        if (!response.ok) {
          throw new Error('Failed to delete description');
        }

    // Only remove if the server delete was successful
    // Remove from the local list
    this.items.splice(index, 1);

    this.deletedDescription = item.description;

    this.isDeletePopupOpen = true;

  } catch (error) {
    console.error(error);
    this.isErrorPopupOpen = true;
  }
    },
    // Closes all popups
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
/* Popup overlay: center alignment and dark background */
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


/* Content inside popups */
.popup-content {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  width: 300px;
  text-align: center;
}

/* Container for headings */
.container {
  max-width: 700px;
  margin: 2rem auto;
  background-color: #fff;
  padding: 2rem;
}

/* Heading and text formatting */
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

/* Input styling */
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

/* Tabel-layout */
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

th, td {
  border-bottom: 1px solid #ccc;
  border-right: 1px solid #ccc;
  padding: 1rem;
  text-align: left;
}

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

/* Delete button */
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

/* Logo size */
.logo {
    max-width: 300px;
}
</style>