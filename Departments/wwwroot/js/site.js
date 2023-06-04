// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {

    let name = document.querySelector("#Name");
    let surname = document.querySelector("#Surname");
    let number = document.querySelector("#Number");
    let age = document.querySelector("#Age");

    let hierarchyItems = document.querySelectorAll("#hierarchy .hierarchy-item");
    let delay = 150; // Задержка между появлением элементов (в миллисекундах)    

    showItemsWithDelay(hierarchyItems, 0);    

    name.addEventListener("keypress", event => {
        eventHandlerCheckLetters(event)
    });

    surname.addEventListener("keypress", event => {
        eventHandlerCheckLetters(event)
    });

    

    age.addEventListener("keypress", (event) => {
        let codeKey = event.which || event.keyCode;
        let inputValue = String.fromCharCode(codeKey);
        if (!/^\d$/.test(inputValue) || !/^\d{0,1}$/.test(event.target.value)) {
            event.preventDefault();
        }        
    });

    number.addEventListener('focus', (event) => {
        if (event.target.value == "") {
            event.target.value = "+";
        }        
    });    

    number.addEventListener("keypress",  (event) => {              

        if (!event.target.value.startsWith("+") && event.target.value != "") {
            event.target.value = "+" + event.target.value;
        }

        let codeKey = event.which || event.keyCode;
        let inputValue = String.fromCharCode(codeKey);        

        if (!/^\d$/.test(inputValue)) {
            event.preventDefault();
        }
        
        if (inputValue.includes("+") || !/^[\d+]{1,12}$/.test(event.target.value)) {               
             event.preventDefault();
        }
    });

    function eventHandlerCheckLetters(event) {

        let codeKey = event.which || event.keyCode;
        let inputValue = String.fromCharCode(codeKey); 

        if (!/^[a-zA-Zа-яА-Я-]$/.test(inputValue)) {
            event.preventDefault();
        }
        if ((event.target.value.includes("-") && inputValue == "-")
                || !/^[a-zA-Zа-яА-Я-]{0,20}$/.test(event.target.value)) {
            event.preventDefault();
        }
    }

    function showItemsWithDelay(items, index) {
        if (index < items.length) {
            let item = items[index];
            item.style.visibility = "visible";
            item.style.animation = "fadeIn .3s ease-in both";

            let subDepartments = item.querySelector(".nested-list");
            if (subDepartments) {
                setTimeout(function () {
                    let subItems = subDepartments.querySelectorAll(".hierarchy-item");
                    showItemsWithDelay(subItems, 0);
                }, delay);
            }

            setTimeout(function () {
                showItemsWithDelay(items, index + 1);
            }, delay);
        }
    }   

})