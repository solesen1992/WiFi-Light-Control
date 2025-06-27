 <!-- Creates the lightSettings page and sets up necessary API calls -->
<!-- Page for light settings with the ability to modify and save the system configuration -->

<!-- HTML -->
<template>
  <div class="container py-4">
    <h2 class="fw-bold mb-4">Lys indstillinger</h2>

    <!-- System ON/OFF switch -->
    <div class="form-check form-switch d-flex justify-content-between align-items-center mb-4">
      <label class="form-check-label" for="systemSwitch">Tænd/sluk hele systemet</label>
      <input class="form-check-input" type="checkbox" id="systemSwitch" v-model="systemOn">
    </div>

    <!-- Time interval: Weekdays -->
<!-- Time period to monitor Wi-Fi signals on weekdays -->
    <div class="mb-3">
      <label class="fw-semibold mb-2">Tidsrum der skal søges efter wi-fi signaler</label>
      <!-- Weekdays: start and end times -->
      <div class="mb-2">
        <strong>Hverdage</strong>
        <div class="d-flex align-items-center gap-2 my-2">
          <!-- If "Whole day" is selected, fields are disabled -->
          <input type="time" class="form-control" v-model="weekdays.start" :disabled="weekdays.wholeDay">
          <span>—</span>
          <input type="time" class="form-control" v-model="weekdays.end" :disabled="weekdays.wholeDay">
        </div>
        <!-- Checkbox: Whole day (00:00-23:59) -->
        <div class="form-check">
          <input class="form-check-input" type="checkbox" v-model="weekdays.wholeDay" id="weekdayWhole" @change="handleWholeDayChange">
          <label class="form-check-label" for="weekdayWhole">Hele dagen</label>
        </div>
      </div>

      <!-- Time interval: Weekends -->
      <div class="mb-2">
        <strong>Weekender</strong>
        <div class="d-flex align-items-center gap-2 my-2">
          <!-- If "Whole day" is selected, fields are disabled -->
          <input type="time" class="form-control" v-model="weekends.start" :disabled="weekends.wholeDay">
          <span>—</span>
          <input type="time" class="form-control" v-model="weekends.end" :disabled="weekends.wholeDay">
        </div>
        <!-- Checkbox: Whole day (00:00-23:59) -->
        <div class="form-check">
          <input class="form-check-input" type="checkbox" v-model="weekends.wholeDay" id="weekendWhole" @change="handleWeekendWholeDayChange">
          <label class="form-check-label" for="weekendWhole">Hele dagen</label>
        </div>
      </div>
    </div>

    <!-- Shutdown time if no user is online on Wi-Fi -->
    <div class="mb-4">
      <label for="timeoutSelect" class="form-label fw-semibold">Slukning af lyset ved ingen Wi-Fi aktivitet</label>
      <select id="timeoutSelect" class="form-select" v-model="timeout">
        <option value="5">Efter 5 minutter</option>
        <option value="10">Efter 10 minutter</option>
        <option value="15">Efter 15 minutter</option>
        <option value="30">Efter 30 minutter</option>
      </select>
    </div>

    <!-- Save button -->
    <div class="text-center">
      <button class="btn btn-primary btn-lg px-4" style="background-color: #7d4dfa; border-color: #7d4dfa;" @click="saveChanges">
        Gem ændringer
      </button>
    </div>
  </div>
</template>

<!-- JAVASCRIPT-->
<script>

import { BASE_URI } from '@/components/baseURIconfig.js';

export default {
  name: 'LightSettings',
  data() {
    return {
      systemOn: true, // Initial system state: system turned on
      // Default times for weekdays and weekends
      weekdays: {
        start: '00:00',
        end: '23:59',
        wholeDay: true
      },
      weekends: {
        start: '00:00',
        end: '23:59',
        wholeDay: true
      },     
      // Default timeout in minutes
      timeout: '15'
    }
  },
  // When the component loads, fetch existing settings
  mounted() {
  this.loadSettings();
  },
  methods: { 
  // When "Whole day" is selected for weekdays
   handleWholeDayChange() {
  if (this.weekdays.wholeDay) {
    this.weekdays.start = '00:00';
    this.weekdays.end = '23:59';
  }
},
// When "Whole day" is selected for weekends
handleWeekendWholeDayChange() {
  if (this.weekends.wholeDay) {
    this.weekends.start = '00:00';
    this.weekends.end = '23:59';
  }
},
  // Remove seconds from time for display
      formatTime(timeStr) {
        return timeStr.substring(0, 5); // removes seconds
},
  // Save changes by sending them to the API
    async saveChanges() {
      // Add seconds to time if missing
      const formatWithSeconds = (time) => time.length === 5 ? time + ':00' : time;

      const payload = {
        systemOnOff: this.systemOn,
        weekdayStartTime: formatWithSeconds(this.weekdays.wholeDay ? "00:00" : this.weekdays.start),
        weekdayEndTime: formatWithSeconds(this.weekdays.wholeDay ? "23:59" : this.weekdays.end),
        weekendStartTime: formatWithSeconds(this.weekends.wholeDay ? "00:00" : this.weekends.start),
        weekendEndTime: formatWithSeconds(this.weekends.wholeDay ? "23:59" : this.weekends.end),
        offlineTimeoutMinutes: parseInt(this.timeout),
        manualOverrideDurationMinutes: 60
};

try {
  // Send a PUT request to `/Settings/saveAll` with current settings as JSON
  const response = await fetch(`${BASE_URI}/Settings/saveAll`, {
    method: 'PUT', // Update existing data
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload) // Convert settings to JSON
  });

  // Throw an error if response is not OK (status 200-299)
  if (!response.ok) throw new Error('Fejl ved gemning');
    // Alert user if save was successful
    alert('Indstillinger gemt!');
  } catch (error) {
    // Log error to console
    console.error(error);
    // Alert user if something went wrong
    alert('Noget gik galt ved gemning!');
  }
},
// Load existing settings from backend
async loadSettings() {
  console.log('Loading settings...');

  try {
    // Send GET request to server to fetch saved settings
    const response = await fetch(`${BASE_URI}/Settings`);

  if (!response.ok) throw new Error('Kunne ikke hente indstillinger');
  // Convert server response to JSON object
  const data = await response.json();
  console.log("Fetched settings", data) // Debug-log

  // Update system on/off status with value from backend
  this.systemOn = data.systemOnOff;

  // Fill in weekday settings:
  this.weekdays = {
    start: this.formatTime(data.weekdayStartTime), // Shorten time (e.g. '08:00:00' → '08:00')
    end: this.formatTime(data.weekdayEndTime),
    wholeDay: data.weekdayStartTime === '00:00:00' && data.weekdayEndTime === '23:59:00'
  };

  // Fill in weekend settings similarly
  this.weekends = {
    start: this.formatTime(data.weekendStartTime),
    end: this.formatTime(data.weekendEndTime),
    wholeDay: data.weekendStartTime === '00:00:00' && data.weekendEndTime === '23:59:00'
  };

  // Save timeout in minutes (e.g. 15) as a string (e.g. "15") for use in <select>`
  this.timeout = data.offlineTimeoutMinutes.toString();

  } catch (error) {
    // Log error and alert if something went wrong
    console.error(error);
    alert('Kunne ikke hente nuværende indstillinger');
  }
}
  
  }
}

</script>

<!-- CSS -->
<style scoped>
/* Changes switch color when turned on */
.form-check-input:checked {
  background-color: #4cd964 !important; /* Apple green */
  border-color: #4cd964 !important;
}
</style>