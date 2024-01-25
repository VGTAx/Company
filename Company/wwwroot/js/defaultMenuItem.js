document.addEventListener("DOMContentLoaded", () => {
   let linkProfile = document.querySelector(".defaultMenuItem");

   // Добавляем событие "load" для выполнения кода после загрузки страницы
   window.addEventListener("load", function () {
      // Вызываем метод click() для элемента
      linkProfile.click();
   });
})

