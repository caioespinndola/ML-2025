
// Adicione este código ao seu arquivo site.js

document.addEventListener('DOMContentLoaded', (event) => {

    // Lógica do Theme Toggle
    const toggleButton = document.getElementById('theme-toggle');
    const body = document.body;

    // Função para aplicar o tema (lendo do localStorage)
    function applyTheme(theme) {
        if (theme === 'dark') {
            body.classList.add('dark-mode');
            toggleButton.innerHTML = '☀️'; // Sol (para ir para o modo light)
        } else {
            body.classList.remove('dark-mode');
            toggleButton.innerHTML = '🌙'; // Lua (para ir para o modo dark)
        }
    }

    // Verifica o tema salvo no navegador assim que a página carrega
    const currentTheme = localStorage.getItem('theme') || 'light';
    applyTheme(currentTheme);

    // Adiciona o evento de clique ao botão
    toggleButton.addEventListener('click', () => {
        // Verifica qual é o tema atual e define o novo
        let newTheme = body.classList.contains('dark-mode') ? 'light' : 'dark';

        // Salva a preferência no navegador
        localStorage.setItem('theme', newTheme);

        // Aplica o novo tema
        applyTheme(newTheme);
    });

});


