function handleRegisterFormValidation() {
    const registerForm = document.getElementById("register_form");

    if (registerForm) {
        registerForm.addEventListener("submit", function (event) {
            event.preventDefault();

            if (!validateRegisterFormLocally()) {
                return;
            }

            validateRegisterFormOnServer(registerForm);
        });
    }
}

function validateRegisterFormLocally() {
    const usernameInput = document.getElementById("username_input");
    const emailInput = document.getElementById("email_input");
    const passwordInput = document.getElementById("password_input");
    const confirmPasswordInput = document.getElementById("passwordconfirm_input");

    if (!validateRequiredFieldsRegisterForm(usernameInput, emailInput, passwordInput, confirmPasswordInput)) {
        return false;
    }

    if (!validateUsername(usernameInput)) {
        return false;
    }

    if (!validateEmail(emailInput)) {
        return false;
    }

    if (!validatePassword(passwordInput, confirmPasswordInput, usernameInput, emailInput)) {
        return false;
    }

    if (!validatePolishChars(usernameInput, emailInput, passwordInput)) {
        return false;
    }

    return true;
}

function validateRequiredFieldsRegisterForm(username, email, password, confirmPassword) {
    if (!username.value || !email.value || !password.value || !confirmPassword.value) {
        openModal("Formularz rejestracji nie jest kompletny. Wypełnij wszystkie puste pola.");
        return false;
    }
    return true;
}

function validateUsername(username) {
    if (username.value.length < 4) {
        openModal("Login musi mieć co najmniej 4 znaki.");
        return false;
    }
    return true;
}

function validateEmail(email) {
    if (!/\S+@\S+\.\S+/.test(email.value)) {
        openModal("Wprowadź poprawny adres e-mail.");
        return false;
    }
    return true;
}

function validatePassword(password, confirmPassword, username, email) {
    const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$/;

    if (password.value.length < 10) {
        openModal("Hasło musi mieć co najmniej 10 znaków.");
        return false;
    }

    if (password.value !== confirmPassword.value) {
        openModal("Hasła muszą być takie same.");
        return false;
    }

    if (!strongPasswordRegex.test(password.value)) {
        openModal("Hasło nie jest dostatecznie silne. Wymagane jest, aby hasło długie na co najmniej 10 znaków zawierało wielkie i małe litery, cyfry i znaki specjalne.");
        return false;
    }

    if (password.value === username.value) {
        openModal("Hasło i login nie mogą być identyczne.");
        return false;
    }

    if (password.value === email.value) {
        openModal("Hasło i mail nie mogą być identyczne.");
        return false;
    }

    return true;
}

function validatePolishChars(username, email, password) {
    const polishCharsRegex = /[ąćęłńóśźżĄĆĘŁŃÓŚŹŻ]/;

    if (polishCharsRegex.test(username.value)) {
        openModal("Login nie może zawierać polskich znaków.");
        return false;
    }

    if (polishCharsRegex.test(email.value)) {
        openModal("Adres e-mail nie może zawierać polskich znaków.");
        return false;
    }

    if (polishCharsRegex.test(password.value)) {
        openModal("Hasło nie może zawierać polskich znaków.");
        return false;
    }

    return true;
}

function validateRegisterFormOnServer(form) {
    console.log("validateOnServerExecuted");
    const formData = new FormData(form);

    fetch('/Home/Register', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(result => {
            if (!result.success) {
                handleServerValidationError(result.errorType);
            } else {
                handleRedirectionAfterRegister("/Home/Login");
            }
        })
        .catch(error => {
            console.error('Błąd komunikacji z serwerem:', error);
            openModal("Wystąpił nieoczekiwany błąd podczas rejestracji.");
        });
}

function handleServerValidationError(errorType) {
    switch (errorType) {
        case "LOGIN_TAKEN":
            openModal("Login jest już zajęty.");
            break;
        case "MAIL_TAKEN":
            openModal("Adres e-mail jest już zajęty.");
            break;
        case "INVALID_FORM":
            openModal("Błędne dane formularza. Sprawdź poprawność danych i spróbuj ponownie.");
            break;

        default:
            openModal("Wystąpił nieoczekiwany błąd.");
            break;
    }
}

function handleRedirectionAfterRegister(redirectUrl) {
    if (redirectUrl) {
        window.location.href = redirectUrl;
    }
}