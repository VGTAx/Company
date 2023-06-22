 //Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
 //for details on configuring this project to bundle and minify static web assets.

 //Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {      

    let hierarchyItems = document.querySelectorAll(".hierarchy .hierarchy-item");
    let delay = 150; // Задержка между появлением элементов (в миллисекундах)    

    showItemsWithDelay(hierarchyItems, 0); 

    function showItemsWithDelay(items, index) {
        if (index < items.length) {
            let item = items[index];
            item.style.visibility = "visible";
            item.style.animation = "fadeIn .3s ease-in both";

            //let subDepartments = item.querySelector(".nested-list");
            //if (subDepartments) {
            //    setTimeout(function () {
            //        let subItems = subDepartments.querySelectorAll(".hierarchy-item");
            //        showItemsWithDelay(subItems, 0);
            //    }, delay);
            //}

            setTimeout(function () {
                showItemsWithDelay(items, index + 1);
            }, delay);
        }
    }   
})