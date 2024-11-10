document.addEventListener("DOMContentLoaded", (event) => {
   let links = document.querySelectorAll(".nav-link.highLightLink");

   function HighlightLink(event) {
      // Удаляем класс "active" у всех ссылок
      links.forEach(function (link) {
         link.classList.remove('active');
      });

      // Добавляем класс "active" для активной ссылки
      event.target.classList.add('active');
   }

   links.forEach(function (link) {
      link.addEventListener('click', HighlightLink);
   }); 
})


