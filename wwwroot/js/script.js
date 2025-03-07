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

let animationInProgress = false;
function handleClicking() {
    const clickerDiv = document.getElementById("clicker");
    const clickCountDisplay = document.getElementById("clickcounter");
    if (!clickerDiv || !clickCountDisplay) {
        return; // stop here if elements are not present on current site
    }

    let clickCount = parseInt(localStorage.getItem("clickCount")) || 0;
    clickCountDisplay.textContent = clickCount;

    clickerDiv.addEventListener("click", function () {
        clickCount++;
        clickCountDisplay.textContent = clickCount;

        if (!animationInProgress) {
            // animation after click
            animationInProgress = true;
            clickCountDisplay.classList.add('clicked');
            requestAnimationFrame(function () {
                setTimeout(function () { clickCountDisplay.classList.remove('clicked'); animationInProgress = false; }, 300);
            });
        }

        localStorage.setItem("clickCount", clickCount);
    });
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