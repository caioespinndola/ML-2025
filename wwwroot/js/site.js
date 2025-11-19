


document.addEventListener('DOMContentLoaded', (event) => {

    
    const toggleButton = document.getElementById('theme-toggle');
    const body = document.body;

    
    function applyTheme(theme) {
        if (theme === 'dark') {
            body.classList.add('dark-mode');
            toggleButton.innerHTML = '☀️'; 
        } else {
            body.classList.remove('dark-mode');
            toggleButton.innerHTML = '🌙'; 
        }
    }

    
    const currentTheme = localStorage.getItem('theme') || 'light';
    applyTheme(currentTheme);

    
    toggleButton.addEventListener('click', () => {
        
        let newTheme = body.classList.contains('dark-mode') ? 'light' : 'dark';

        
        localStorage.setItem('theme', newTheme);

        
        applyTheme(newTheme);
    });

});


