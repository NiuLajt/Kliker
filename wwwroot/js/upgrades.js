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
        const div = document.createElement('div');
        div.className = 'upgrade';
        div.innerHTML = `
            <h3>${upgrade.name}</h3>
            <p>${upgrade.description}</p>
            <br />
            <span>Wymagany poziom: ${upgrade.levelRequired}</span>
        `;
        container.appendChild(div);
    });
}
