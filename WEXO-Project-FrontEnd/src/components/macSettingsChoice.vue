 <!-- Creates the MacSettings page and sets up necessary API calls -->

 <!-- HTML -->
<template>
    <!-- Header and introduction -->
    <div class="container mb-5">
        <h1>
        Vedligeholdelse af lys-indstillinger
    </h1>
    <h5>
        Tilføj MAC adresser som skal frasorteres af systemet
    </h5>
    </div>

    <!-- Form to add a new MAC address -->
    <div>
        <form @submit.prevent="addItem">
            <input v-model="newItem.macAdress" placeholder="MAC adresser" required/>
            <button class="addButton" type="Submit">Tilføj</button>
        </form>

    <!-- Error popup on API failure -->
    <div v-if="isErrorPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Error!</h3>
        <p>Der opstod et problem, venligst prøv igen</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

     <!-- Success popup on successful addition -->
     <div v-if="isSuccessPopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Success!</h3>
        <p>MAC adressen "{{ addedMac }}" er blevet tilføjet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Success popup for deletion -->
    <div v-if="isDeletePopupOpen" class="popup-overlay" @click="closePopup">
      <div class="popup-content" @click.stop>
        <h3>Deleted!</h3>
        <p>MAC adressen "{{ deletedMac }}" er blevet slettet.</p>
        <button @click="closePopup" class="btn btn-secondary">Close</button>
      </div>
    </div>

    <!-- Table showing existing MAC addresses -->
        <table>
            <thead>
                <tr>
                    <th>MAC adresser</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in items" :key="index">
                    <td>{{ item.macAdress }}</td>
                    <td>
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
        macAdress: '' // The MAC address entered in the form
      },
      items: [], // List of existing MAC addresses
      isSuccessPopupOpen: false,
      isErrorPopupOpen: false,
      isDeletePopupOpen: false,
      deletedMac: '', // Used in popup text
      addedMac: ''
    };
  },
  mounted() {
    this.loadItems(); // Load MAC addresses when the component mounts
  },
  methods: {
    // Add a new entry to the table
    // Sends a new MAC address to the backend, shows a success popup, and updates the local list
    async addItem() {
        try {

            if (this.newItem.macAdress) {
                const response = await fetch(`${BASE_URI}/Blacklist/macAdress`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    macAdress: this.newItem.macAdress
                })
            });

        if (!response.ok) {
            throw new Error('Error with connecting and posting item')
        }


        this.items.push({
            macAdress: this.newItem.macAdress
        });

        this.addedMac = this.newItem.macAdress;

        // Clear the input fields
        this.newItem.macAdress = '';

        this.isSuccessPopupOpen = true;
      }
        } catch (error) {
            console.error('Failed to post item:', error)
            this.isErrorPopupOpen = true;
        }
    },

    // Load data (existing MAC addresses) from API
    async loadItems() {
      try {
        const response = await fetch(`${BASE_URI}/Blacklist/macAdress`);
        const data = await response.json();
        console.log("Fetched data:", data);
        this.items = data;
      } catch (error) {
        console.error('Failed to fetch items:', error);
      }
    },

    // Remove an entry (MAC address) from the table based on index
    // Deletes a MAC address both on the server and from the local list. Shows a popup on success or error
    async removeItem(index) {
  try {
    const item = this.items[index];

    if (!item || !item.macAdress) {
      throw new Error("Item not found or has no host name");
    }

    const response = await fetch(`${BASE_URI}/Blacklist/macAdress/${encodeURIComponent(item.macAdress)}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error('Failed to delete description');
    }

    // Only remove if the server delete was successful
    this.items.splice(index, 1);

    this.deletedMac = item.macAdress;

    this.isDeletePopupOpen = true;

  } catch (error) {
    console.error(error);
    this.isErrorPopupOpen = true;
  }
    },
    // Closes all popups. Used when the user clicks “Close”
    closePopup() {
      this.isSuccessPopupOpen = false;
      this.isErrorPopupOpen = false;
      this.isDeletePopupOpen = false;
  }
  }
};
</script>

<!-- CSS -->
<style scoped>
/* Basic styling for popup */

/* Dark background and centering of popup window */
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

/* Popup window appearance */
.popup-content {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  width: 300px;
  text-align: center;
}

/* General Container Styling */
.container {
  max-width: 700px;
  margin: 2rem auto;
  background-color: #fff;
  padding: 2rem;
}

/* Headings */
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

/* Form */
form {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
  max-width: 1280px; /* or whatever width you want */
  flex-wrap: wrap;  /* if items should wrap on small screens */
}

/* Input fields */
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

/* Submit-button */
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

/* Table and contents */
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

/* Last column in each row: for buttons */
td:last-child, th:last-child {
  width: 100px; /* or whatever fits your button nicely */
  text-align: center;
}

/* Button in table row */
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


/* Remove border from table header */
thead th {
  border: none;
}

/* Delete button with different color */
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
  background-color: #c9164c; /* Darker red on hover */
}

/* Logo max width */
.logo {
    max-width: 300px;
}
</style>