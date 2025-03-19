let clickCount = 0;
let unsavedClicks = 0;
let animationInProgress = false;
let saveTimeout;

async function fetchUserPoints() {
    try {
        const response = await fetch("/Home/UserData");
        const data = await response.json();

        if (data.success) {
            clickCount = data.points;
            unsavedClicks = 0;
            updateClickDisplay();
        } else {
            console.error("Błąd pobierania danych użytkownika:", data.errorType);
        }
    } catch (error) {
        console.error("Błąd komunikacji z serwerem:", error);
    }
}


function updateClickDisplay() {
    const clickCountDisplay = document.getElementById("clickcounter");
    if (clickCountDisplay) {
        clickCountDisplay.textContent = clickCount;
    }
}


function getUsername() {
    const usernameElement = document.getElementById('username_header');
    return usernameElement ? usernameElement.textContent.trim() : null;
}


async function saveUserPoints() {
    if (unsavedClicks === 0) return;

    const username = getUsername();

    if (!username) {
        console.error("Brak nazwy użytkownika – nie można zapisać punktów!");
        return;
    }

    try {
        const response = await fetch("/Home/UpdatePoints", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, points: clickCount }),
        });

        const data = await response.json();
        if (!data.success) {
            console.error("Błąd zapisu punktów:", data.errorType);
        } else {
            unsavedClicks = 0;
        }
    } catch (error) {
        console.error("Błąd komunikacji z serwerem:", error);
    }
}

function handleClickingUser() { 
    const clickerDiv = document.getElementById("clicker");
    if (!clickerDiv) return;

    fetchUserPoints();

    clickerDiv.addEventListener("click", function () {
        clickCount++;
        unsavedClicks++;
        updateClickDisplay();
        playClickAnimation();

        if (unsavedClicks >= 5) {
            saveUserPoints();
        } else {
            clearTimeout(saveTimeout);
            saveTimeout = setTimeout(saveUserPoints, 5000);
        }
    });
}

function updateUserData() {
    const currentPath = window.location.pathname;
    if (currentPath != "/Home/Dashboard") {
        return;
    }

    fetch('/Home/UserData')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById('username_header').textContent = data.username;
                document.getElementById('level_header').textContent = data.level + ' lvl';
                document.getElementById('level_progress').value = data.level;
                document.getElementById('clickcounter').textContent = data.points;
            } else {
                console.error('Błąd podczas pobierania danych użytkownika:', data.errorType);
            }
        })
        .catch(error => {
            console.error('Wystąpił błąd podczas wykonywania zapytania AJAX:', error);
        });
}

function updatePointsValueInDatabase() {
    setInterval(function () {
        const username = getUsername();
        if (!username) return;

        fetch('/Home/UpdatePoints', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, points: clickCount })
        })
            .then(response => response.json())
            .then(data => {
                if (!data.success) {
                    console.error("Błąd aktualizacji punktów:", data.errorType);
                }
            });
    }, 10000);
}
