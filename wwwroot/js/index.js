
function handleClickingGuest() {
    const clickerDiv = document.getElementById("clicker");
    const clickCountDisplay = document.getElementById("clickcounter");
    if (!clickerDiv || !clickCountDisplay) return;

    let clickCount = parseInt(localStorage.getItem("clickCount")) || 0;
    clickCountDisplay.textContent = clickCount;

    clickerDiv.addEventListener("click", function () {
        clickCount++;
        clickCountDisplay.textContent = clickCount;
        playClickAnimation();
        localStorage.setItem("clickCount", clickCount);
    });
}

function handleNavigationGuest() {
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