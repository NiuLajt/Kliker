﻿function handleNavigationToRegister() {
    const registerButton = document.getElementById("register_button");
    if (registerButton) {
        registerButton.addEventListener("click", function () {
            window.location.href = "/Home/Register";
        });
    }
}

function handleLoginFormValidation() {
    const loginForm = document.getElementById("login_form");

    if (loginForm) {
        loginForm.addEventListener("submit", function (event) {
            event.preventDefault();

            if (!validateLoginFormLocally()) {
                return;
            }

            validateLoginFormOnServer(loginForm);
        });
    }
}

function validateLoginFormLocally() {
    const usernameInput = document.getElementById("username_input");
    const passwordInput = document.getElementById("password_input");

    if (!validateRequiredFieldsLoginForm(usernameInput, passwordInput)) {
        return false;
    }

    return true;
}

function validateRequiredFieldsLoginForm(username, password) {
    if (!username.value || !password.value) {
        openModal("Formularz logowania nie jest kompletny. Wypełnij wszystkie puste pola.");
        return false;
    }
    return true;
}

function validateLoginFormOnServer(form) {
    const formData = new FormData(form);

    fetch('/Home/Login', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                handleRedirectionAfterLogin(result.redirectUrl);
            } else {
                handleServerLoginError(result.errorType);
            }
        })
        .catch(error => {
            console.error('Błąd komunikacji z serwerem:', error);
            openModal("Wystąpił nieoczekiwany błąd podczas logowania.");
        });
}


function handleServerLoginError(errorType) {
    switch (errorType) {
        case "INVALID_FORM":
            openModal("Formularz nieprawidłowy.");
            break;
        case "INVALID_CREDENTIALS":
            openModal("Błędne dane logowania. Sprawdź login i hasło.");
            break;0
        case "USER_NOT_FOUND":
            openModal("Nie znaleziono użytkownika.");
            break;

        default:
            openModal("Wystąpił nieoczekiwany błąd.");
            break;
    }
}

function handleRedirectionAfterLogin(redirectUrl) {
    if (redirectUrl) {
        window.location.href = redirectUrl;
    }
}