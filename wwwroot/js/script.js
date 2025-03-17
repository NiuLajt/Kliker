function handleNavigation() {
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
}

function handleModal() {
    const modal = document.getElementById("errorModal");
    const modalMessage = document.getElementById("modalMessage");
    const modalCloseButton = document.getElementById("okModalButton");
    const modalCloseCrossButton = document.getElementById("closeModal");

    function openModal(message) {
        modalMessage.textContent = message;
        modal.style.display = "block";
    }

    function closeModal() {
        modal.style.display = "none";
    }

    if (modalCloseButton) {
        modalCloseButton.addEventListener("click", closeModal);
    }

    if (modalCloseCrossButton) {
        modalCloseCrossButton.addEventListener("click", closeModal);
    }

    window.openModal = openModal;
}

document.addEventListener("DOMContentLoaded", function () {
    handleNavigation();
    handleClicking();
    handleModal();
    handleRegisterFormValidation();
    handleLoginFormValidation();
});