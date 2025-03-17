
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