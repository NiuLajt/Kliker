animationInProgress = false;
function playClickAnimation() {
    const clickCountDisplay = document.getElementById("clickcounter");
    if (!clickCountDisplay || animationInProgress) return;
    animationInProgress = true;
    clickCountDisplay.classList.add('clicked');
    
    setTimeout(() => {
        clickCountDisplay.classList.remove('clicked');
        animationInProgress = false;
    }, 300);
}

function handleClicking() {
    const currentPath = window.location.pathname;

    if (currentPath === "/") {
        handleClickingGuest();
    } else if (currentPath === "/Home/Dashboard") {
        handleClickingUser();
    }
}

function handleNavigation() {
    const currentPath = window.location.pathname;

    if (currentPath === "/") {
        handleNavigationGuest();
    } else if (currentPath === "/Home/Dashboard") {
        //handleNavigationUser(); do zrobienia w przyszłości
    }
}

function handleValidation() {
    const currentPath = window.location.pathname;

    if (currentPath === "/Home/Login") {
        handleLoginFormValidation();
    } else if (currentPath === "/Home/Register") {
        handleRegisterFormValidation();
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

// main program - invoke functions when everything is loaded
document.addEventListener("DOMContentLoaded", function () {
    handleNavigation();
    handleClicking();
    handleValidation();
    handleModal();
    updateUserData();
    updatePointsValueInDatabase();
});