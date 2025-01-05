// Example JavaScript functionality for the buttons
function togglePasswordVisibility() {
    const passwordField = document.getElementById('Password');
    const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordField.setAttribute('type', type);
}
