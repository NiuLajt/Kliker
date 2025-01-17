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

document.addEventListener("DOMContentLoaded", function () {
    handleNavigation();
    handleClicking();
});