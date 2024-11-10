document.addEventListener('DOMContentLoaded', () => {

   let hierarchyItems = document.querySelectorAll(".hierarchy .hierarchy-item");
   let delay = 150; // Задержка между появлением элементов (в миллисекундах)    

   ShowItemsWithDelay(hierarchyItems, 0);

   function ShowItemsWithDelay(items, index) {
      if (index < items.length) {
         let item = items[index];
         item.style.visibility = "visible";
         item.style.animation = "fadeIn .3s ease-in both";

         setTimeout(function () {
            ShowItemsWithDelay(items, index + 1);
         }, delay);
      }
   }
})