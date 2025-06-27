<!-- Login page handling login -->

 <!-- HTML -->
 <template>
    <div class="login-container"> <!-- Centers all content -->
      <div class="login-box"> <!-- Box where the login form is displayed -->
        <img src="/assets/images/wexo.svg" alt="WEXO Logo" class="logo" /> <!-- Logo -->
        <h2>Login</h2>

        <!-- 
      Form with prevent, so the page does not reload
      Normally an HTML form reloads the page when you press "submit".
      This is the default behavior of <form>.
      But in Single Page Applications (SPA) you don't want the page to reload because you handle login yourself.
      Therefore, we use @submit.prevent to: prevent page reload + run a JavaScript method instead (handleLogin in this case)
      -->
        <form @submit.prevent="handleLogin">

          <!-- Input field for username.
          v-model provides two-way binding between the input field and the username variable in data().
          This means the field and variable are always synchronized -->
          <div class="input-wrapper">
            <input v-model="username" type="text" placeholder="Username" required />
          </div>
          <!-- Input field for password.
          Also bound with v-model so the password variable updates automatically -->
          <div class="input-wrapper">
            <input v-model="password" type="password" placeholder="Password" required />
          </div>
           <!-- Login button that triggers form submit (without reload) -->
          <button type="submit" class="login-button">Login</button>
          <!-- Shows error message if login fails -->
          <p v-if="error" class="error">{{ error }}</p>
        </form>
      </div>
    </div>
  </template>
  
  <!-- JAVASCRIPT-->
  <script>
  // Import base API URL
  import { BASE_URI } from '@/components/baseURIconfig.js';

  export default {
    data() {
      return {
        username: '', // Stores entered username
        password: '', // Stores entered password
        error: null // Stores any error messages
      };
    },
    methods: {
      // Method handling login, called when form is submitted
      async handleLogin() {
        this.error = null; //Reset previous errors
        try {
          // Sends POST request with username and password to backend
          const res = await fetch(`${BASE_URI}/Login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username: this.username, password: this.password })
          });
  
          // If login is approved
          if (res.status === 200) {
            localStorage.setItem('authenticated', 'true'); // Stores that user is logged in
            this.$emit('login-success'); // Emits event to show main page

            // If login fails (wrong username or password)
          } else if (res.status === 401) {
            this.error = 'Invalid username or password';

            // Other unexpected errors
          } else {
            this.error = `Unexpected error: ${res.status}`;
          }
        } catch {
          this.error = 'Could not connect to the server';
        }
      }
    }
  };
  </script>
  
  <!-- CSS -->
  <style scoped>
  /* Scoped styles for the login form */

  /* The whole login container fills the screen height, centers content horizontally and vertically */
  .login-container {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    background: #f4f4f4;
  }
  
   /* The box where the login form is shown */
  .login-box {
    width: 100%; /* Fills full width, but... */
    max-width: 400px; /* ...no more than 400px */
    background: white;
    padding: 30px;
    border-radius: 18px; /* Rounded corners for the login box */
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    text-align: center;
    display: flex;
    flex-direction: column; /* Stack elements vertically */
    align-items: stretch; /* Ensure all children are stretched */
  }
  
  /* Logo size and margin below */
  .logo {
    height: 80px;
    margin-bottom: 20px;
  }
  
  /* Wrapper around input fields */
  .input-wrapper {
    position: relative;
    margin-top: 10px;
  }
  
  /* Styling input fields (both username and password) */
  input {
    width: 100%;
    padding: 10px;
    border-radius: 8px; /* Rounded corners for input fields */
    border: 1px solid #ccc;
    box-sizing: border-box;
    transition: border 0.3s ease;
  }
  
  /* When input field is focused */
  input:focus {
    border-color: #007bff; /* Change border color on focus */
    outline: none;
  }
  
  /* Adds extra spacing between username and password */
  input[type="password"] {
    margin-top: 15px; /* Adds extra space between username and password */
  }
  
  /* Form element styled with flexbox to ensure good layout structure */
  form {
    display: flex;
    flex-direction: column;
    align-items: stretch;
    justify-content: center; /* Vertically center the form items */
  }
  
  /* Error messages shown in red with some margin on top */
  .error {
    color: red;
    margin-top: 10px;
  }
  
  /* Scoped styles for the login button */
  .login-button {
    width: 100%;
    padding: 12px;
    margin-top: 20px;
    background-color: #7d4dfa !important; /* Use !important to override Bootstrap's styles */
    color: white !important; /* Ensure the text is white */
    border: none !important; /* Remove any border from Bootstrap's default styles */
    border-radius: 8px !important; /* Rounded corners */
    cursor: pointer !important;
    font-size: 16px !important;
    transition: background-color 0.3s ease, transform 0.2s ease; /* Adding transform for click effect */
    display: inline-block;
    text-align: center;
    outline: none !important;
  }
  
  /* When user hovers over the button */
  .login-button:hover {
    background-color: #5935b8 !important; /* Hover effect */
  }
  
  /* When user clicks the button */
  .login-button:active {
    opacity: 0.8 !important; /* Click effect */
    transform: scale(0.98); /* Slight scale down on click */
  }
  </style>
  