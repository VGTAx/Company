document.addEventListener('DOMContentLoaded', () => {

    let name = document.querySelector("#Name");
    let surname = document.querySelector("#Surname");
    let number = document.querySelector("#Number");
    let age = document.querySelector("#Age");
        
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

    number.addEventListener("keypress", (event) => {

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

})