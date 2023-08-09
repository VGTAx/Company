document.addEventListener("DOMContentLoaded", () => {
    // Обработчик события обновления данных
    function updateDataHandler(data) {
        document.querySelector('#NameAccount').textContent = data; 
    }

    // Подписка на событие обновления данных
    document.addEventListener('dataUpdated', function (e) {
        let data = 'Привет, ' + e.detail.data;
        updateDataHandler(data);
    });
})

