document.addEventListener('DOMContentLoaded', () => {
    const button = document.getElementById('loadMessage');
    const message = document.getElementById('message');

    if (!button || !message) {
        return;
    }

    button.addEventListener('click', () => {
        message.textContent = 'Static JavaScript is successfully loaded from wwwroot.';
    });
});
