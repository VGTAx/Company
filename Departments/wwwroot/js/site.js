// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    let number = document.getElementById("Number");
    let age = document.getElementById("Age");

    age.addEventListener("keypress", (e) => {
        let codeKey = e.which || e.keyCode;
        let inputValue = String.fromCharCode(codeKey);

        if (!/^\d$/.test(inputValue) ||
            !/^\d{0,2}$/.test(e.target.value)) {
            e.preventDefault();
        }        
    });

    number.addEventListener('focus', function () {
        if (event.target.value == "") {
            event.target.value = "+";
        }        
    });    

    number.addEventListener("keypress", function (e) {              

        if (!e.target.value.startsWith("+") && e.target.value != "") {
            e.target.value = "+" + e.target.value;
        }

        let codeKey = e.which || e.keyCode;
        let inputValue = String.fromCharCode(codeKey);        

        if (!/^\d$/.test(inputValue)) {
            e.preventDefault();
        }
        
        if (inputValue.includes("+") || !/^[\d+]{1,12}$/.test(e.target.value)) {               
             e.preventDefault();
        }
    });

})