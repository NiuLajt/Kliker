document.addEventListener("DOMContentLoaded", function () {
    const loginButton = document.getElementById("login_button");
    if (loginButton) {
        loginButton.addEventListener("click", function () {
            window.location.href = "/Home/Login";
        });
    }

    const registerButton = document.getElementById("register_button");
    if (registerButton) {
        registerButton.addEventListener("click", function () {
            window.location.href = "/Home/Register";
        });
    }
});