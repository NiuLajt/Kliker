async function loadUpgrades() {
    try {
        const response = await fetch('/Home/UpgradesUnlockedAndNot');
        if (!response.ok) {
            console.error('Problem!!!');
            return;
        }

        const result = await response.json();
        const upgrades = result.upgradesList;
        renderUpgrades(upgrades);
    } catch (error) {
        console.error('Wystąpił problem:', error);
    }
}

function renderUpgrades(upgrades) {
    const container = document.getElementById('upgradeslower_panel');
    container.innerHTML = '';

    upgrades.forEach(upgrade => {
        let upgradeMessage;
        let upgradeTileClass;
        if (upgrade.isAlreadyUnlocked) {
            upgradeMessage = "Odblokowano :)";
            upgradeTileClass = "unlocked";
        }
        else {
            upgradeMessage = "Kliknij, by odblokować";
        }

        const div = document.createElement('div');
        div.className = 'upgrade';
        div.innerHTML = `
            <h3 class="upgrade_header ${upgradeTileClass}">${upgrade.name}</h3>
            <p>${upgrade.description}</p>
            <h3 class="upgradedunlockinfo_header">${upgradeMessage}</h3>
            <span>Wymagany poziom: ${upgrade.levelRequired}</span>
        `;
        container.appendChild(div);
    });
}

function handleUnlockingUpgrades() {
    document.getElementById('upgradeslower_panel').addEventListener("click", function (event) {
        let clickedTile = event.target.closest(".upgrade");
        if (clickedTile) {
            let nameOfClickedUpgrade = clickedTile.querySelector(".upgrade_header").textContent;
            if (!clickedTile.querySelector(".upgradeunlockinfo_header").textContent === "Odblokowano :)") {
                if (tryUpgrade(nameOfClickedUpgrade)) {
                    clickedTile.classList.add("unlocked");
                    clickedTile.querySelector(".upgradeunlockinfo_header").textContent = "Odblokowano :)";
                }
            }
        }
    });
}

async function tryUpgrade(name) {
    // ask server if this user can unlock this upgrade
    try {
        const response = await fetch('/Home/UpgradeStatus');
        if (!response.ok) {
            console.error('Problem!!!');
            return false;
        }

        result = response.json();

        if (result.status === "LEVEL_TOO_LOW") {
            openModal("Twój poziom jest za niski!");
            return false;
        }

        if (result.status === "ALREADY_UNLOCKED") {
            openModal("Osiągnięcie już zostało odblokowane.");
            return false;
        }

        if (result.status === "UNLOCKING_FINISHED") {
            return true;
        }
    } catch (error) {
        console.error('Wystąpił problem:', error);
    }
}