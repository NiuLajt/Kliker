function updateUserData() {
    fetch('/Home/UserData')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById('username_header').textContent = data.username;
                document.getElementById('level_header').textContent = data.level + ' lvl';
                document.getElementById('level_progress').value = data.level;
                document.getElementById('clickcounter').value = data.points;
            } else {
                console.error('Błąd podczas pobierania danych użytkownika:', data.errorType);
            }
        })
        .catch(error => {
            console.error('Wystąpił błąd podczas wykonywania zapytania AJAX:', error);
        });
}